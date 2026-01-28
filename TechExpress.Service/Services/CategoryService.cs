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

        //===============================Category Service Delete Handling===============================
        // Path: TechExpress.Service/Services/CategoryService.cs
        public async Task<string> HandleDeleteCategory(Guid id)
        {
            // 1. Kiểm tra tồn tại với Tracking để EF có thể xóa/sửa
            var category = await _unitOfWork.CategoryRepository.FindCategoryByIdWithTrackingAsync(id)
                           ?? throw new NotFoundException("Không tìm thấy danh mục để xóa");

            // 2. Kiểm tra ràng buộc dữ liệu (Validation Usage)
            bool hasChildCategories = await _unitOfWork.CategoryRepository.AnyChildCategoriesAsync(id);
            bool hasLinkedProducts = await _unitOfWork.ProductRepository.AnyProductsInCategoryAsync(id);

            // 3. Thực hiện logic xóa
            if (hasChildCategories || hasLinkedProducts)
            {
                // --- THỰC HIỆN SOFT DELETE (XÓA MỀM) ---
                // Nếu đã xóa mềm rồi thì không cần làm lại
                if (category.IsDeleted)
                    throw new BadRequestException("Danh mục này đã ngừng hoạt động từ trước.");

                category.IsDeleted = true; // Chuyển Status sang Inactive
                category.UpdatedAt = DateTimeOffset.Now; // Ghi nhận thời điểm xóa vào UpdateAt

                await _unitOfWork.SaveChangesAsync();
                return "Danh mục có dữ liệu liên quan (sản phẩm hoặc con) nên đã được chuyển sang trạng thái Inactive.";
            }
            else
            {
                // --- THỰC HIỆN HARD DELETE (XÓA CỨNG) ---
                // Xóa vĩnh viễn vì không có ràng buộc
                _unitOfWork.CategoryRepository.Remove(category);
                await _unitOfWork.SaveChangesAsync();

                return "Danh mục chưa có dữ liệu liên kết nên đã được xóa vĩnh viễn khỏi hệ thống.";
            }
        }
    }
}