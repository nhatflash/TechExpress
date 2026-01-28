using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
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