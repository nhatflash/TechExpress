using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechExpress.Application.Common;
using TechExpress.Application.DTOs.Requests;
using TechExpress.Application.DTOs.Responses;
using TechExpress.Service;


namespace TechExpress.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ServiceProviders _serviceProvider;

        public CategoryController(ServiceProviders serviceProviders)
        {
            _serviceProvider = serviceProviders;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            // Lấy từng field từ request để truyền vào Service
            var category = await _serviceProvider.CategoryService.HandleCreateCategory(
                request.Name,
                request.Description,
                request.ParentCategoryId,
                request.ImageUrl
            );

            var response = ResponseMapper.MapToCategoryResponseFromCategory(category);
            return Ok(ApiResponse<CategoryResponse>.OkResponse(response));
        }
    }
}
