using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechExpress.Application.Common;
using TechExpress.Application.Dtos.Requests;
using TechExpress.Application.Dtos.Responses;
using TechExpress.Service;
using TechExpress.Service.Utils;

namespace TechExpress.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SpecDefinitionController : ControllerBase
{
    private readonly ServiceProviders _serviceProvider;

    public SpecDefinitionController(ServiceProviders serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Tạo định nghĩa thông số mới
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateSpecDefinitionRequest request)
    {
        var specDefinition = await _serviceProvider.SpecDefinitionService.HandleCreateAsync(
            request.Code,
            request.Name,
            request.CategoryId,
            request.Unit,
            request.AcceptValueType,
            request.Description,
            request.IsRequired);

        var response = ResponseMapper.MapToSpecDefinitionResponseFromSpecDefinition(specDefinition);
        return CreatedAtAction(nameof(GetById), new { id = specDefinition.Id }, ApiResponse<SpecDefinitionResponse>.CreatedResponse(response));
    }

    /// <summary>
    /// Lấy danh sách định nghĩa thông số có phân trang
    /// </summary>
    /// <param name="pageNumber">Số trang (mặc định: 1)</param>
    /// <param name="pageSize">Kích thước trang (mặc định: 20, tối đa: 100)</param>
    /// <param name="searchName">Tìm kiếm theo tên (contains)</param>
    /// <param name="createdFrom">Lọc từ thời điểm tạo (CreatedAt &gt;= createdFrom)</param>
    /// <param name="createdTo">Lọc đến thời điểm tạo (CreatedAt &lt;= createdTo)</param>
    [HttpGet]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? searchName = null,
        [FromQuery] DateTimeOffset? createdFrom = null,
        [FromQuery] DateTimeOffset? createdTo = null)
    {
        var pagination = await _serviceProvider.SpecDefinitionService.HandleGetPagedAsync(
            pageNumber,
            pageSize,
            searchName,
            createdFrom,
            createdTo);
        var response = ResponseMapper.MapToSpecDefinitionResponsePaginationFromSpecDefinitionPagination(pagination);
        return Ok(ApiResponse<Pagination<SpecDefinitionResponse>>.OkResponse(response));
    }

    /// <summary>
    /// Lấy thông tin chi tiết của một định nghĩa thông số
    /// </summary>
    /// <param name="id">ID của định nghĩa thông số</param>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var specDefinition = await _serviceProvider.SpecDefinitionService.HandleGetByIdAsync(id);
        var response = ResponseMapper.MapToSpecDefinitionResponseFromSpecDefinition(specDefinition);
        return Ok(ApiResponse<SpecDefinitionResponse>.OkResponse(response));
    }

    /// <summary>
    /// Cập nhật thông tin định nghĩa thông số
    /// </summary>
    /// <param name="id">ID của định nghĩa thông số</param>
    /// <param name="request">Dữ liệu cập nhật</param>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSpecDefinitionRequest request)
    {
        var specDefinition = await _serviceProvider.SpecDefinitionService.HandleUpdateAsync(
            id,
            request.Code,
            request.Name,
            request.CategoryId,
            request.Unit,
            request.AcceptValueType,
            request.Description,
            request.IsRequired);

        var response = ResponseMapper.MapToSpecDefinitionResponseFromSpecDefinition(specDefinition);
        return Ok(ApiResponse<SpecDefinitionResponse>.OkResponse(response));
    }

    /// <summary>
    /// Xóa định nghĩa thông số (xóa mềm nếu có dữ liệu liên quan, xóa cứng nếu không)
    /// </summary>
    /// <param name="id">ID của định nghĩa thông số</param>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _serviceProvider.SpecDefinitionService.HandleDeleteAsync(id);
        return Ok(ApiResponse<string>.OkResponse("Định nghĩa thông số đã được xóa thành công."));
    }

    [HttpGet("category/{categoryId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> FindSpecDefinitionsOnCategory([FromRoute] Guid categoryId, [FromQuery] int pageNumber)
    {
        var pagedSpecs =
            await _serviceProvider.SpecDefinitionService.HandleFindSpecDefinitionsOnCategoryIdAsync(categoryId,
                pageNumber);
        var response = ResponseMapper.MapToSpecDefinitionResponsePaginationFromSpecDefinitionPagination(pagedSpecs);
        return Ok(ApiResponse<Pagination<SpecDefinitionResponse>>.OkResponse(response));
    }
}
