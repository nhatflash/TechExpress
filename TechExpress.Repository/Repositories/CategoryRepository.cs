using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TechExpress.Repository.Contexts;
using TechExpress.Repository.Models;

namespace TechExpress.Repository.Repositories
{
    public class CategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task AddCategoryAsync(Category category)
        {
             await _context.Categories.AddAsync(category);
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> FindCategoryByIdAsync(Guid id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        // Thêm phương thức này để hỗ trợ Update (có tracking)
        public async Task<Category?> FindCategoryByIdWithTrackingAsync(Guid id)
            => await _context.Categories.AsTracking().FirstOrDefaultAsync(c => c.Id == id);

    }
}
