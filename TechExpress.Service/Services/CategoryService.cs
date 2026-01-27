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

        public async Task<Category> HandleUpdateCategory(
             Guid id,
             string? name = null,
             string? description = null,
             Guid? parentCategoryId = null,
             string? imageUrl = null,
             bool? status = null)
        {
            // 1. Tìm danh mục hiện tại
            var category = await _unitOfWork.CategoryRepository.FindCategoryByIdWithTrackingAsync(id)
                           ?? throw new NotFoundException("Không tìm thấy danh mục để cập nhật");

            string finalName = name ?? category.Name;

            // 2. Kiểm tra tính duy nhất TOÀN BẢNG
            if (name != null)
            {
                // Kiểm tra xem cái tên mới này đã có ai dùng chưa (trên toàn bộ DB)
                bool isDuplicate = await _unitOfWork.CategoryRepository.IsNameGlobalExistsAsync(id, finalName);
                if (isDuplicate) throw new BadRequestException("Tên danh mục này đã tồn tại trên hệ thống");
            }

            // 3. Chống vòng lặp (Giữ nguyên logic cũ)
            if (parentCategoryId != null && parentCategoryId != category.ParentCategoryId)
            {
                if (parentCategoryId == id) throw new BadRequestException("Danh mục không thể làm cha chính nó");
                if (await IsDescendant(id, parentCategoryId.Value))
                    throw new BadRequestException("Lỗi vòng lặp: Không thể chọn danh mục con làm danh mục cha");

                category.ParentCategoryId = parentCategoryId;
            }

            // 4. Cập nhật từng dòng (Partial Update)
            if (!string.IsNullOrWhiteSpace(name)) category.Name = name;
            if (!string.IsNullOrWhiteSpace(description)) category.Description = description;
            if (imageUrl != null) category.ImageUrl = imageUrl;
            if (status.HasValue) category.IsDeleted = !status.Value;

            // 5. Lưu thay đổi và ghi Audit
            category.UpdatedAt = DateTimeOffset.Now;
            await _unitOfWork.SaveChangesAsync(); // Lúc này sẽ không còn lỗi SqlException 2601 nữa

            return category;
        }
        // Hàm bổ trợ kiểm tra đệ quy vòng lặp
        private async Task<bool> IsDescendant(Guid rootId, Guid targetParentId)
        {
            var childrenIds = await _unitOfWork.CategoryRepository.GetChildCategoryIdsAsync(rootId);
            foreach (var childId in childrenIds)
            {
                if (childId == targetParentId) return true;
                if (await IsDescendant(childId, targetParentId)) return true;
            }
            return false;
        }
    }
}