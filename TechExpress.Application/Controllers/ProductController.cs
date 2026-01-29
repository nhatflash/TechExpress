using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechExpress.Application.Common;
using TechExpress.Application.Dtos.Requests;
using TechExpress.Application.Dtos.Responses;
using TechExpress.Service;
using TechExpress.Service.Utils;

namespace TechExpress.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ServiceProviders _serviceProvider;

        public ProductController(ServiceProviders serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetProductList([FromQuery] ProductFilterRequest request)
        {
            if (request.Page < 1)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Page must be greater than 0"
                });
            }

            var productPagination = await _serviceProvider.ProductService
                .HandleGetProductListWithPaginationAsync(
                    request.Page,
                    request.PageSize,
                    request.SortBy,
                    request.Search,
                    request.CategoryId,
                    request.Status
                );

            var response = ResponseMapper
                .MapToProductListResponsePaginationFromProductPagination(productPagination);

            return Ok(ApiResponse<Pagination<ProductListResponse>>.OkResponse(response));
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetProductDetail(Guid id)
        {
            var product = await _serviceProvider.ProductService
                .HandleGetProductDetailAsync(id);

            var response = ResponseMapper
                .MapToProductDetailResponseFromProduct(product);

            return Ok(ApiResponse<ProductDetailResponse>.OkResponse(response));
        }


        [HttpPost("create-product")]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResponse<ProductListResponse>>> CreateProduct([FromForm] CreateProductRequest request)
        {
            // Convert spec values (DTO tầng controller) -> primitive type cho service
            var specDict = request.SpecValues?
                .Where(x => x != null)
                .GroupBy(x => x.SpecDefinitionId)
                .ToDictionary(g => g.Key, g => g.Last().Value?.Trim() ?? string.Empty);

            var product = await _serviceProvider.ProductService.HandleCreateProduct(
                request.Name.Trim(),
                request.Sku.Trim(),
                request.CategoryId,
                request.Price,
                request.StockQty,
                request.Status,
                request.Description.Trim(),
                request.Images,
                specDict
            );

            var response = ResponseMapper.MapToProductDetailResponseFromProduct(product);

            return CreatedAtAction(nameof(CreateProduct), ApiResponse<ProductDetailResponse>.CreatedResponse(response));
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResponse<ProductDetailResponse>>> UpdateProduct(
        Guid id,
        [FromForm] UpdateProductRequest request)
        {
            var specDict = request.SpecValues?
                .Where(x => x != null)
                .GroupBy(x => x.SpecDefinitionId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Last().Value?.Trim() ?? string.Empty
                );

            var updated = await _serviceProvider.ProductService.HandleUpdateProduct(
                id,
                request.Name.Trim(),
                request.Sku.Trim(),
                request.CategoryId,
                request.Price,
                request.StockQty,
                request.Status,
                request.Description.Trim(),
                request.NewImages,
                request.DeletedImageIds,
                specDict
            );

            var response = ResponseMapper.MapToProductDetailResponseFromProduct(updated);
            return Ok(ApiResponse<ProductDetailResponse>.OkResponse(response));
        }



        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _serviceProvider.ProductService.HandleDeleteProductAsync(id);
            return Ok(ApiResponse<string>.OkResponse("Xóa sản phẩm thành công."));
        }





    }
}


