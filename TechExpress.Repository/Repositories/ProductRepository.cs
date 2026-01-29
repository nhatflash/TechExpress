using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TechExpress.Repository.Contexts;

namespace TechExpress.Repository.Repositories
{
    public class ProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<bool> AnyProductsInCategoryAsync(Guid id)
        {
            // Kiểm tra xem có sản phẩm nào đang thuộc danh mục này không
            return await _context.Products.AnyAsync(p => p.CategoryId == id);
        }
    }
}
