using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechExpress.Application.Common;
using TechExpress.Application.DTOs.Responses;
using TechExpress.Service;
using TechExpress.Service.DTOs.Requests;

namespace TechExpress.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ServiceProviders _service;

        public CategoryController(ServiceProviders serviceProviders)
        {
            _service = serviceProviders;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            var category = await _service.CategoryService.HandleCreateCategory(request);
            var response = ResponseMapper.MapToCategoryResponseFromCategory(category);
            return Ok(ApiResponse<CategoryResponse>.OkResponse(response));
        }
    }
}
