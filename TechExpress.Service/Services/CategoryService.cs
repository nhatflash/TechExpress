using System;
using System.Collections.Generic;
using System.Text;
using TechExpress.Repository;
using TechExpress.Repository.CustomExceptions;
using TechExpress.Repository.Models;
using TechExpress.Service.DTOs.Requests;

namespace TechExpress.Service.Services
{
    public class CategoryService
    {
        private readonly UnitOfWork _unitOfWork;

        public CategoryService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Category> HandleCreateCategory(CreateCategoryRequest request)
        {
            // 1. Kiểm tra danh mục cha (nếu có)
            if (request.ParentCategoryId.HasValue)
            {
                var parent = await _unitOfWork.CategoryRepository.FindCategoryByIdAsync(request.ParentCategoryId.Value);
                if (parent == null) throw new NotFoundException("Danh mục cha không tồn tại");
            }

            // 2. Tạo đối tượng Model
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                ParentCategoryId = request.ParentCategoryId,
                Description = request.Description,
                ImageUrl = request.ImageUrl,

                // Mặc định luôn là Active khi tạo mới (IsDeleted = false)
                IsDeleted = false,
                UpdatedAt = DateTimeOffset.Now
            };

            // 3. Lưu vào database
            await _unitOfWork.CategoryRepository.AddCategoryAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return category;
        }
    }
}