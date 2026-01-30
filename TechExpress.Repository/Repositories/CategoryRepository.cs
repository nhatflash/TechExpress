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


        //không phân biệt cấp cha con. do bên DB nó set Uni que nên chỉ cần check tên toàn cục
        public async Task<bool> IsNameGlobalExistsAsync(Guid id, string name)
        {
            // Kiểm tra xem có danh mục nào khác (khác ID hiện tại) trùng tên này không
            return await _context.Categories.AnyAsync(c =>
                c.Id != id &&
                c.Name.ToLower() == name.ToLower());
        }

        public async Task<List<Guid>> GetChildCategoryIdsAsync(Guid parentId)
        {
            // Lấy tất cả ID của các danh mục con trực tiếp
            return await _context.Categories
                .Where(c => c.ParentCategoryId == parentId)
                .Select(c => c.Id)
                .ToListAsync();
        }


        public IQueryable<Category> GetCategoriesQueryable()
        {
            return _context.Categories.AsNoTracking().AsQueryable();
        }

        public async Task<bool> AnyChildCategoriesAsync(Guid id)
        {
            // Kiểm tra xem có danh mục nào đang nhận ID này làm cha không
            return await _context.Categories.AnyAsync(c => c.ParentCategoryId == id);
        }

        public async Task<bool> AnyActiveChildCategoriesAsync(Guid id)
        {
            // Kiểm tra xem có danh mục con nào chưa bị xóa mềm (IsDeleted == false) không
            return await _context.Categories.AnyAsync(c => c.ParentCategoryId == id && !c.IsDeleted);
        }
        public void Remove(Category entity)
        {
            // Sử dụng DbSet của EF Core để xóa
            _context.Categories.Remove(entity);
        }
    }
}
