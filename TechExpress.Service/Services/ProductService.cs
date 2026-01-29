using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TechExpress.Repository;
using TechExpress.Repository.CustomExceptions;
using TechExpress.Repository.Enums;
using TechExpress.Repository.Models;
using TechExpress.Service.Utils;

namespace TechExpress.Service.Services
{
    public class ProductService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(UnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
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
            string? search,
            Guid? categoryId,
            ProductStatus? status)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 20;

            var (products, totalCount) = await _unitOfWork.ProductRepository
                .GetProductsPagedAsync(
                    page,
                    pageSize,
                    sortBy,
                    search,
                    categoryId,
                    status
                );

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
              List<IFormFile>? images,
              Dictionary<Guid, string>? specValues)
        {
            //basic validate
            if (string.IsNullOrWhiteSpace(name)) throw new BadRequestException("Tên sản phẩm không được để trống.");
            if (string.IsNullOrWhiteSpace(sku)) throw new BadRequestException("SKU không được để trống.");
            if (price <= 0) throw new BadRequestException("Giá sản phẩm phải lớn hơn 0.");
            if (stockQty < 0) throw new BadRequestException("Số lượng tồn kho phải >= 0.");
            if (string.IsNullOrWhiteSpace(description)) throw new BadRequestException("Mô tả không được để trống.");

            name = name.Trim();
            sku = sku.Trim();
            description = description.Trim();

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
            var providedIds = (specValues ?? new Dictionary<Guid, string>()).Keys.ToHashSet();

            var missing = requiredIds.Except(providedIds).ToList();
            if (missing.Any())
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
                Status = status,
                Description = description,
                UpdatedAt = DateTimeOffset.Now
            };

            await using var tx = await _unitOfWork.BeginTransactionAsync();

            await _unitOfWork.ProductRepository.AddProductAsync(product);

            // Images
            if (images != null && images.Count > 0)
            {
                var imageEntities = await SaveProductImagesAsync(product.Id, images);
                foreach (var img in imageEntities)
                    product.Images.Add(img);
            }

            // SpecValues
            if (specValues != null && specValues.Count > 0)
            {
                foreach (var kv in specValues)
                {
                    var specId = kv.Key;
                    var raw = kv.Value;

                    if (!specDefMap.TryGetValue(specId, out var def))
                        throw new BadRequestException("Có SpecDefinition không hợp lệ hoặc không thuộc Category đã chọn.");

                    var psv = BuildProductSpecValue(product.Id, def, raw);
                    product.SpecValues.Add(psv);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            await tx.CommitAsync();

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

        private async Task<List<ProductImage>> SaveProductImagesAsync(Guid productId, List<IFormFile> images)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            const long maxFileSize = 5 * 1024 * 1024;

            var webRootPath = _webHostEnvironment.WebRootPath;
            if (string.IsNullOrEmpty(webRootPath))
            {
                webRootPath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot");
                if (!Directory.Exists(webRootPath))
                    Directory.CreateDirectory(webRootPath);
            }

            var uploadsFolder = Path.Combine(webRootPath, "uploads", "products", productId.ToString());
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var httpContext = _httpContextAccessor.HttpContext;
            var baseUrl = httpContext != null
                ? $"{httpContext.Request.Scheme}://{httpContext.Request.Host}"
                : "https://localhost:7194";

            var results = new List<ProductImage>();

            foreach (var file in images)
            {
                if (file == null || file.Length == 0) continue;

                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(ext))
                    throw new BadRequestException("Loại ảnh không hợp lệ (jpg, jpeg, png, gif, webp).");

                if (file.Length > maxFileSize)
                    throw new BadRequestException("Ảnh quá lớn. Tối đa 5MB.");

                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var url = $"{baseUrl}/uploads/products/{productId}/{fileName}";

                results.Add(new ProductImage
                {
                    ProductId = productId,
                    ImageUrl = url
                });
            }

            return results;
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
            List<IFormFile>? newImages,
            List<long>? deletedImageIds,
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

            var providedIds = new HashSet<Guid>();
            foreach (var id in existingMap.Keys) providedIds.Add(id);
            if (specValues != null)
            {
                foreach (var id in specValues.Keys) providedIds.Add(id);
            }

            var missing = requiredIds.Except(providedIds).ToList();
            if (missing.Any())
                throw new BadRequestException("Thiếu thông số bắt buộc cho sản phẩm.");

            await using var tx = await _unitOfWork.BeginTransactionAsync();

            // Update base fields
            product.Name = name;
            product.Sku = sku;
            product.CategoryId = categoryId;
            product.Price = price;
            product.Stock = stockQty;
            product.Status = status;
            product.Description = description;

            product.UpdatedAt = DateTimeOffset.Now;

            if (deletedImageIds != null && deletedImageIds.Count > 0)
            {
                await _unitOfWork.ProductImageRepository
                    .DeleteByIdsAsync(deletedImageIds);
            }

            if (newImages != null && newImages.Count > 0)
            {
                var imageEntities = await SaveProductImagesAsync(productId, newImages);

                if (imageEntities.Count > 0)
                {
                    await _unitOfWork.ProductImageRepository.AddRangeAsync(imageEntities);
                }
            }

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

            await _unitOfWork.SaveChangesAsync();
            await tx.CommitAsync();

            var updated = await _unitOfWork.ProductRepository.FindByIdAsync(productId)
                ?? throw new NotFoundException("Không tìm thấy sản phẩm sau khi cập nhật.");

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
                .FindByIdWithTrackingAsync(productId)
                ?? throw new NotFoundException("Không tìm thấy sản phẩm.");

            await using var tx = await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _unitOfWork.ProductRepository.HardDeleteProductByIdAsync(productId);

                await _unitOfWork.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch (DbUpdateException ex)
            {
                await tx.RollbackAsync();

                if (product.Status != ProductStatus.Unavailable)
                {
                    product.Status = ProductStatus.Unavailable;
                    product.UpdatedAt = DateTimeOffset.Now;
                }

                await _unitOfWork.SaveChangesAsync();
            }
        }


    }
}
    
