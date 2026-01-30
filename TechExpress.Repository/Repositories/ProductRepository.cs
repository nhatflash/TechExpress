using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TechExpress.Repository.Contexts;
using TechExpress.Repository.Enums;
using TechExpress.Repository.Models;

namespace TechExpress.Repository.Repositories
{
    public class ProductRepository
    {
        private readonly ApplicationDbContext _context;


        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private IQueryable<Product> BuildFilteredQuery(string? search, Guid? categoryId, ProductStatus? status)
        {
            var query = _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Images)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (status.HasValue)
                query = query.Where(p => p.Status == status.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(s) ||
                    p.Sku.ToLower().Contains(s));
            }

            return query;
        }

        private async Task<(List<Product> Products, int TotalCount)> ExecutePagedQueryAsync(
            IQueryable<Product> query, int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, totalCount);
        }

        public async Task<(List<Product> Products, int TotalCount)> FindProductsPagedSortByPriceAsync(
            int page, int pageSize, string? search, Guid? categoryId, ProductStatus? status)
        {
            var query = BuildFilteredQuery(search, categoryId, status).OrderBy(p => p.Price);
            return await ExecutePagedQueryAsync(query, page, pageSize);
        }

        public async Task<(List<Product> Products, int TotalCount)> FindProductsPagedSortByCreatedAtAsync(
            int page, int pageSize, string? search, Guid? categoryId, ProductStatus? status)
        {
            var query = BuildFilteredQuery(search, categoryId, status).OrderBy(p => p.CreatedAt);
            return await ExecutePagedQueryAsync(query, page, pageSize);
        }

        public async Task<(List<Product> Products, int TotalCount)> FindProductsPagedSortByStockQtyAsync(
            int page, int pageSize, string? search, Guid? categoryId, ProductStatus? status)
        {
            var query = BuildFilteredQuery(search, categoryId, status).OrderBy(p => p.Stock);
            return await ExecutePagedQueryAsync(query, page, pageSize);
        }

        public async Task<(List<Product> Products, int TotalCount)> FindProductsPagedSortByUpdatedAtAsync(
            int page, int pageSize, string? search, Guid? categoryId, ProductStatus? status)
        {
            var query = BuildFilteredQuery(search, categoryId, status).OrderBy(p => p.UpdatedAt);
            return await ExecutePagedQueryAsync(query, page, pageSize);
        }

        public async Task<bool> ExistsBySkuAsync(string sku)
        {
            var s = sku.Trim().ToLower();
            return await _context.Products.AnyAsync(p => p.Sku.ToLower() == s);
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task<Product?> FindByIdAsync(Guid id)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.SpecValues)
                    .ThenInclude(sv => sv.SpecDefinition)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product?> FindByIdWithTrackingAsync(Guid id)
        {
            return await _context.Products
                .AsTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product?> FindByIdWithNoTrackingAsync(Guid id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
        }


        public async Task<bool> ExistsBySkuAsync(string sku, Guid excludeProductId)
        {
            var s = sku.Trim().ToLower();

            return await _context.Products.AnyAsync(p =>
                p.Id != excludeProductId &&
                p.Sku.ToLower() == s
            );
        }
        //public Task<bool> IsProductUsedAsync(Guid productId)
        //{
        //    return Task.FromResult(true);
        //}

        //
        //Add-Migration Init -StartupProject TechExpress.Application -Project TechExpress.Repository
        //Update-Database -StartupProject TechExpress.Application -Project TechExpress.Repository

        public async Task HardDeleteProductByIdAsync(Guid productId)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product != null)
                _context.Products.Remove(product);
        }




        public async Task<bool> AnyProductsInCategoryAsync(Guid id)
        {
            // Kiểm tra xem có sản phẩm nào đang thuộc danh mục này không
            return await _context.Products.AnyAsync(p => p.CategoryId == id);
        }

    }
}
