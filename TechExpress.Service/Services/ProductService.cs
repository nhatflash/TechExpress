using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechExpress.Repository;
using TechExpress.Repository.CustomExceptions;
using TechExpress.Repository.Enums;
using TechExpress.Repository.Models;
using TechExpress.Service.Enums;
using TechExpress.Service.Utils;

namespace TechExpress.Service.Services
{
    public class ProductService
    {
        private readonly UnitOfWork _unitOfWork;

        public ProductService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Product> HandleGetProductDetailAsync(Guid productId)
        {
            var product = await _unitOfWork.ProductRepository.FindByIdAsync(productId)
                ?? throw new NotFoundException("Không tìm thấy sản phẩm.");

            return product;
        }


        public async Task<Pagination<Product>> HandleGetProductListWithPaginationAsync(
    int page,
    int pageSize,
    ProductSortBy sortBy,
    SortDirection sortDirection,
    string? search,
    Guid? categoryId,
    ProductStatus? status)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 20;

            var isDescending = sortDirection == SortDirection.Desc;

            List<Guid>? categoryIds = null;
            if (categoryId.HasValue)
            {
                var descendants = await _unitOfWork.CategoryRepository
                    .GetDescendantCategoryIdsAsync(categoryId.Value);

                categoryIds = new List<Guid>(descendants.Count + 1) { categoryId.Value };
                categoryIds.AddRange(descendants);
            }

            var (products, totalCount) = sortBy switch
            {
                ProductSortBy.Price => await _unitOfWork.ProductRepository
                    .FindProductsPagedSortByPriceAsync(page, pageSize, isDescending, search, categoryIds, status),

                ProductSortBy.CreatedAt => await _unitOfWork.ProductRepository
                    .FindProductsPagedSortByCreatedAtAsync(page, pageSize, isDescending, search, categoryIds, status),

                ProductSortBy.StockQty => await _unitOfWork.ProductRepository
                    .FindProductsPagedSortByStockQtyAsync(page, pageSize, isDescending, search, categoryIds, status),

                _ => await _unitOfWork.ProductRepository
                    .FindProductsPagedSortByUpdatedAtAsync(page, pageSize, isDescending, search, categoryIds, status)
            };

            return new Pagination<Product>
            {
                Items = products,
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<Product> HandleCreateProduct(
              string name,
              string sku,
              Guid categoryId,
              decimal price,
              int stockQty,
              ProductStatus status,
              string description,
              List<string>? imageUrls,
              Dictionary<Guid, string>? specValues)
        {
            //basic validate
            if (string.IsNullOrWhiteSpace(name)) throw new BadRequestException("Tên sản phẩm không được để trống.");
            if (string.IsNullOrWhiteSpace(sku)) throw new BadRequestException("SKU không được để trống.");
            if (price <= 0) throw new BadRequestException("Giá sản phẩm phải lớn hơn 0.");
            if (stockQty < 0) throw new BadRequestException("Số lượng tồn kho phải >= 0.");
            if (string.IsNullOrWhiteSpace(description)) throw new BadRequestException("Mô tả không được để trống.");

            //check availablity
            if (await _unitOfWork.ProductRepository.ExistsBySkuAsync(sku))
                throw new BadRequestException("SKU đã tồn tại.");

            var category = await _unitOfWork.CategoryRepository.FindCategoryByIdAsync(categoryId);
            if (category == null || category.IsDeleted)
                throw new NotFoundException("Không tìm thấy danh mục.");

            var specDefs = await _unitOfWork.SpecDefinitionRepository.GetByCategoryIdAsync(categoryId);
            var specDefMap = specDefs.ToDictionary(x => x.Id, x => x);

            //Validate required specs
            var requiredIds = specDefs.Where(x => x.IsRequired).Select(x => x.Id).ToHashSet();

            var missingRequired = requiredIds
                .Where(id =>
                    specValues == null
                    || !specValues.TryGetValue(id, out var val)
                    || string.IsNullOrWhiteSpace(val))
                .ToList();

            if (missingRequired.Any())
                throw new BadRequestException("Thiếu thông số bắt buộc cho sản phẩm.");

            // Create product 
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = name,
                Sku = sku,
                CategoryId = categoryId,
                Price = price,
                Stock = stockQty,
                WarrantyMonth = 1,
                Status = status,
                Description = description,
                UpdatedAt = DateTimeOffset.Now
            };

            //await using var tx = await _unitOfWork.BeginTransactionAsync();

            await _unitOfWork.ProductRepository.AddProductAsync(product);

            // Images (URLs)
            if (imageUrls != null && imageUrls.Count > 0)
            {
                var imageEntities = imageUrls
                    .Where(url => !string.IsNullOrWhiteSpace(url))
                    .Select(url => new ProductImage
                    {
                        ProductId = product.Id,
                        ImageUrl = url.Trim()
                    })
                    .ToList();

                if (imageEntities.Count > 0)
                {
                    await _unitOfWork.ProductImageRepository.AddRangeAsync(imageEntities);
                }
            }

            // SpecValues
            if (specValues != null && specValues.Count > 0)
            {
                foreach (var kv in specValues)
                {
                    var specId = kv.Key;
                    var raw = kv.Value;

                    if (!specDefMap.TryGetValue(specId, out var def))
                        //throw new BadRequestException("Có SpecDefinition không hợp lệ hoặc không thuộc Category đã chọn.");
                        continue;

                    var psv = BuildProductSpecValue(product.Id, def, raw);
                    product.SpecValues.Add(psv);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            //await tx.CommitAsync();

            // load 
            var created = await _unitOfWork.ProductRepository.FindByIdAsync(product.Id)
                ?? throw new NotFoundException("Không tìm thấy sản phẩm vừa tạo.");

            return created;
        }

        private ProductSpecValue BuildProductSpecValue(Guid productId, SpecDefinition def, string rawValue)
        {
            rawValue = (rawValue ?? "").Trim();
            if (string.IsNullOrWhiteSpace(rawValue))
                throw new BadRequestException($"Giá trị '{def.Name}' không được để trống.");

            var sv = new ProductSpecValue
            {
                ProductId = productId,
                SpecDefinitionId = def.Id,
                UpdatedAt = DateTimeOffset.Now
            };

            switch (def.AcceptValueType)
            {
                case SpecAcceptValueType.Text:
                    sv.TextValue = rawValue;
                    break;

                case SpecAcceptValueType.Number:
                    if (!int.TryParse(rawValue, out var i))
                        throw new BadRequestException($"'{def.Name}' phải là số nguyên.");
                    sv.NumberValue = i;
                    break;

                case SpecAcceptValueType.Decimal:
                    if (!decimal.TryParse(rawValue, out var d))
                        throw new BadRequestException($"'{def.Name}' phải là số thập phân.");
                    sv.DecimalValue = d;
                    break;

                case SpecAcceptValueType.Bool:
                    if (!bool.TryParse(rawValue, out var b))
                        throw new BadRequestException($"'{def.Name}' phải là true/false.");
                    sv.BoolValue = b;
                    break;

                default:
                    throw new BadRequestException($"Kiểu dữ liệu '{def.AcceptValueType}' chưa được hỗ trợ.");
            }

            return sv;
        }

        
        public async Task<Product> HandleUpdateProduct(
            Guid productId,
            string name,
            string sku,
            Guid categoryId,
            decimal price,
            int stockQty,
            ProductStatus status,
            string description,
            Dictionary<Guid, string>? specValues)

        {
            if (string.IsNullOrWhiteSpace(name)) throw new BadRequestException("Tên sản phẩm không được để trống.");
            if (string.IsNullOrWhiteSpace(sku)) throw new BadRequestException("SKU không được để trống.");
            if (price <= 0) throw new BadRequestException("Giá sản phẩm phải lớn hơn 0.");
            if (stockQty < 0) throw new BadRequestException("Số lượng tồn kho phải >= 0.");
            if (string.IsNullOrWhiteSpace(description)) throw new BadRequestException("Mô tả không được để trống.");

            name = name.Trim();
            sku = sku.Trim();
            description = description.Trim();

            var product = await _unitOfWork.ProductRepository.FindByIdWithTrackingAsync(productId)
                ?? throw new NotFoundException("Không tìm thấy sản phẩm.");

            if (await _unitOfWork.ProductRepository.ExistsBySkuAsync(sku, excludeProductId: productId))
                throw new BadRequestException("SKU đã tồn tại.");

            var category = await _unitOfWork.CategoryRepository.FindCategoryByIdAsync(categoryId);
            if (category == null || category.IsDeleted)
                throw new NotFoundException("Không tìm thấy danh mục.");

            var specDefs = await _unitOfWork.SpecDefinitionRepository.GetByCategoryIdAsync(categoryId);
            var specDefMap = specDefs.ToDictionary(x => x.Id, x => x);

            var existingSpecValues = await _unitOfWork.ProductSpecValueRepository
                .GetByProductIdWithTrackingAsync(productId);

            var existingMap = existingSpecValues.ToDictionary(x => x.SpecDefinitionId, x => x);

            // Validate required specs:
            // - consider "provided" = keys sent by user + keys already existing in DB
            var requiredIds = specDefs.Where(x => x.IsRequired).Select(x => x.Id).ToHashSet();

            //var providedIds = new HashSet<Guid>();
            //foreach (var id in existingMap.Keys) providedIds.Add(id);
            //if (specValues != null)
            //{
            //    foreach (var id in specValues.Keys) providedIds.Add(id);
            //}

            //var missing = requiredIds.Except(providedIds).ToList();
            //if (missing.Any())
            //    throw new BadRequestException("Thiếu thông số bắt buộc cho sản phẩm.");

            //await using var tx = await _unitOfWork.BeginTransactionAsync();

            // Update base fields
            product.Name = name;
            product.Sku = sku;
            product.CategoryId = categoryId;
            product.Price = price;
            product.Stock = stockQty;
            product.Status = status;
            product.Description = description;

            product.UpdatedAt = DateTimeOffset.Now;

            // Upsert spec values (skip nếu spec def không tồn tại / không thuộc category)
            if (specValues != null && specValues.Count > 0)
            {
                foreach (var kv in specValues)
                {
                    var specId = kv.Key;
                    var raw = kv.Value;

                    if (!specDefMap.TryGetValue(specId, out var def))
                        continue;

                    if (existingMap.TryGetValue(specId, out var current))
                    {
                        ApplyTypedValue(current, def, raw);
                        current.UpdatedAt = DateTimeOffset.Now;
                    }
                    else
                    {
                        var created = BuildProductSpecValue(productId, def, raw);
                        await _unitOfWork.ProductSpecValueRepository.AddAsync(created);
                    }
                }
            }

            var specDefIds = specDefs.Select(x => x.Id).ToHashSet();
            var missingSpecDefIds = specDefIds.Except(existingMap.Keys).ToList();

            if (missingSpecDefIds.Count > 0)
            {
                foreach (var specDefId in missingSpecDefIds)
                {
                    var def = specDefMap[specDefId];

                    //if (def.IsRequired)
                    //    throw new BadRequestException($"Thiếu thông số bắt buộc: {def.Name}");

                    var newSv = new ProductSpecValue
                    {
                        ProductId = productId,
                        SpecDefinitionId = def.Id,
                        UpdatedAt = DateTimeOffset.Now
                    };

                    switch (def.AcceptValueType)
                    {
                        case SpecAcceptValueType.Text:
                            newSv.TextValue = string.Empty;
                            break;
                        case SpecAcceptValueType.Number:
                            newSv.NumberValue = 0;
                            break;
                        case SpecAcceptValueType.Decimal:
                            newSv.DecimalValue = 0m;
                            break;
                        case SpecAcceptValueType.Bool:
                            newSv.BoolValue = false;
                            break;
                        default:
                            // nếu bạn muốn: throw new BadRequestException(...)
                            continue;
                    }

                    await _unitOfWork.ProductSpecValueRepository.AddAsync(newSv);
                    existingMap[specDefId] = newSv;
                }
            }

            await _unitOfWork.SaveChangesAsync();
            //await tx.CommitAsync();

            var updated = await _unitOfWork.ProductRepository.FindByIdAsync(productId)
                ?? throw new NotFoundException("Không tìm thấy sản phẩm sau khi cập nhật.");

            return updated;
        }

        public async Task<Product> HandleReplaceProductImagesAsync(
            Guid productId,
            List<string>? imageUrls)
        {
            var product = await _unitOfWork.ProductRepository
                .FindByIdWithTrackingAsync(productId)
                ?? throw new NotFoundException("Không tìm thấy sản phẩm.");

            //await using var tx = await _unitOfWork.BeginTransactionAsync();

            await _unitOfWork.ProductImageRepository.DeleteByProductIdAsync(productId);

            if (imageUrls != null && imageUrls.Count > 0)
            {
                var images = new List<ProductImage>();
                foreach (var url in imageUrls)
                {
                    if (string.IsNullOrWhiteSpace(url)) continue;

                    images.Add(new ProductImage
                    {
                        ProductId = productId,
                        ImageUrl = url.Trim()
                    });
                }

                if (images.Count > 0)
                {
                    await _unitOfWork.ProductImageRepository.AddRangeAsync(images);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            //await tx.CommitAsync();

            var updated = await _unitOfWork.ProductRepository.FindByIdAsync(productId)
                ?? throw new NotFoundException("Không tìm thấy sản phẩm sau khi cập nhật ảnh.");

            return updated;
        }

        private void ApplyTypedValue(ProductSpecValue sv, SpecDefinition def, string rawValue)
        {
            rawValue = (rawValue ?? "").Trim();
            if (string.IsNullOrWhiteSpace(rawValue))
                throw new BadRequestException($"Giá trị '{def.Name}' không được để trống.");

            sv.TextValue = null;
            sv.NumberValue = null;
            sv.DecimalValue = null;
            sv.BoolValue = null;

            switch (def.AcceptValueType)
            {
                case SpecAcceptValueType.Text:
                    sv.TextValue = rawValue;
                    break;

                case SpecAcceptValueType.Number:
                    if (!int.TryParse(rawValue, out var i))
                        throw new BadRequestException($"'{def.Name}' phải là số nguyên.");
                    sv.NumberValue = i;
                    break;

                case SpecAcceptValueType.Decimal:
                    if (!decimal.TryParse(rawValue, out var d))
                        throw new BadRequestException($"'{def.Name}' phải là số thập phân.");
                    sv.DecimalValue = d;
                    break;

                case SpecAcceptValueType.Bool:
                    if (!bool.TryParse(rawValue, out var b))
                        throw new BadRequestException($"'{def.Name}' phải là true/false.");
                    sv.BoolValue = b;
                    break;

                default:
                    throw new BadRequestException($"Kiểu dữ liệu '{def.AcceptValueType}' chưa được hỗ trợ.");
            }
        }

        //public async Task HandleDeleteProductAsync(Guid productId)
        //{
        //    var product = await _unitOfWork.ProductRepository.FindByIdWithTrackingAsync(productId)
        //        ?? throw new NotFoundException("Không tìm thấy sản phẩm.");

        //    var isUsed = await _unitOfWork.ProductRepository.IsProductUsedAsync(productId);

        //    await using var tx = await _unitOfWork.BeginTransactionAsync();

        //    if (isUsed)
        //    {

        //        if (product.Status == ProductStatus.Unavailable)
        //            return;

        //        product.Status = ProductStatus.Unavailable;
        //        product.UpdatedAt = DateTimeOffset.Now;

        //        // product.DeletedAt = DateTimeOffset.Now;
        //    }
        //    else
        //    {
        //        await _unitOfWork.ProductRepository.HardDeleteProductByIdAsync(productId);
        //    }

        //    await _unitOfWork.SaveChangesAsync();
        //    await tx.CommitAsync();
        //}

        public async Task HandleDeleteProductAsync(Guid productId)
        {
            var product = await _unitOfWork.ProductRepository
                .FindByIdWithNoTrackingAsync(productId)
                ?? throw new NotFoundException("Không tìm thấy sản phẩm.");

            //await using var tx = await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _unitOfWork.ProductRepository.HardDeleteProductByIdAsync(productId);

                await _unitOfWork.SaveChangesAsync();
                //await tx.CommitAsync();
            }
            catch (DbUpdateException)
            {
                //await tx.RollbackAsync();

                if (product.Status != ProductStatus.Unavailable)
                {
                    product.Status = ProductStatus.Unavailable;
                    product.UpdatedAt = DateTimeOffset.Now;
                }

                await _unitOfWork.SaveChangesAsync();
            }
        }


    }

    //#Add-Migration Init -StartupProject TechExpress.Application -Project TechExpress.Repository
//Update-Database -StartupProject TechExpress.Application -Project TechExpress.Repository
}

