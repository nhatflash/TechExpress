using System;
using System.Collections.Generic;
using System.Text;
using TechExpress.Repository;
using TechExpress.Repository.CustomExceptions;
using TechExpress.Repository.Models;


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
    }
}