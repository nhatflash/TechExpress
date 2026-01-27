using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TechExpress.Repository;
using TechExpress.Repository.CustomExceptions;
using TechExpress.Repository.Models;
using TechExpress.Service.Utils;


namespace TechExpress.Service.Services
{
    public class CategoryService
    {
        private readonly UnitOfWork _unitOfWork;

        public CategoryService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Category> HandleCreateCategory(
            string name,
            string description,
            Guid? parentCategoryId,
            string? imageUrl)
        {
            // 1. Kiểm tra danh mục cha (nếu có)
            if (parentCategoryId.HasValue)
            {
                var parent = await _unitOfWork.CategoryRepository.FindCategoryByIdAsync(parentCategoryId.Value);
                if (parent == null) throw new NotFoundException("Danh mục cha không tồn tại");
            }

            // 2. Khởi tạo và gán từng thuộc tính
            // Lưu ý: 'Name' và 'Description' là 'required' nên phải gán ngay lúc 'new'
            var category = new Category
            {
                Name = name,
                Description = description
            };

            // Gán các thuộc tính còn lại từng dòng một
            category.Id = Guid.NewGuid();
            category.ParentCategoryId = parentCategoryId;
            category.ImageUrl = imageUrl;
            category.IsDeleted = false; // Luôn mặc định là Active (IsDeleted = false)
            category.UpdatedAt = DateTimeOffset.Now;

            // 3. Lưu vào database thông qua Repository và UnitOfWork
            await _unitOfWork.CategoryRepository.AddCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return category;
        }
        //=======================================Category List Service =======================================//
        public async Task<Pagination<Category>> HandleGetCategories(
            string? searchName,
            Guid? parentId,
            bool? status,
            int pageNumber)
        {
            const int pageSize = 20;

            // 1. Kiểm tra điều kiện biên cho số trang
            if (pageNumber < 1) pageNumber = 1;

            var query = _unitOfWork.CategoryRepository.GetCategoriesQueryable();

            // 2. Lọc theo Name (Thêm Trim để xử lý khoảng trắng)
            if (!string.IsNullOrWhiteSpace(searchName))
            {
                string cleanSearch = searchName.Trim().ToLower();
                query = query.Where(c => c.Name.ToLower().Contains(cleanSearch));
            }

            // 3. Lọc theo ParentId
            if (parentId.HasValue)
            {
                query = query.Where(c => c.ParentCategoryId == parentId);
            }

            // 4. Lọc theo Status
            if (status.HasValue)
            {
                query = query.Where(c => c.IsDeleted == !status.Value);
            }

            // 5. Sắp xếp
            query = query.OrderByDescending(c => c.CreatedAt);

            // 6. Tính toán tổng số bản ghi
            var totalCount = await query.CountAsync();

            // Kiểm tra nếu tổng số bản ghi bằng 0 thì trả về luôn để tiết kiệm tài nguyên
            if (totalCount == 0)
            {
                return new Pagination<Category> { Items = new List<Category>(), TotalCount = 0, PageNumber = pageNumber, PageSize = pageSize };
            }

            // 7. Thực hiện phân trang
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new Pagination<Category>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }
    }
}