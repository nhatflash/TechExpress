using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechExpress.Application.Common;
using TechExpress.Application.Dtos.Requests;
using TechExpress.Application.Dtos.Responses;
using TechExpress.Service;

namespace TechExpress.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer,Staff,Admin")]
    public class CartController : ControllerBase
    {
        private readonly ServiceProviders _serviceProvider;

        public CartController(ServiceProviders serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Get current user's cart with all items
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var cart = await _serviceProvider.CartService.HandleGetCurrentCartAsync();
            var response = ResponseMapper.MapToCartResponseFromCart(cart);
            return Ok(ApiResponse<CartResponse>.OkResponse(response));
        }

        /// <summary>
        /// Get list of cart items for the current user's active cart (Customer only)
        /// </summary>
        [HttpGet("items")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetCartItems()
        {
            var cart = await _serviceProvider.CartService.HandleGetCurrentCartAsync();
            var response = ResponseMapper.MapToCartResponseFromCart(cart);
            
            if (cart.Id == Guid.Empty || cart.Items.Count == 0)
            {
                return Ok(ApiResponse<List<CartItemResponse>>.OkResponse([]));
            }

            return Ok(ApiResponse<List<CartItemResponse>>.OkResponse(response.Items));
        }

        /// <summary>
        /// Add a product to cart
        /// </summary>
        [HttpPost("items")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            var cart = await _serviceProvider.CartService.HandleAddProductToCartAsync(
                request.ProductId,
                request.Quantity
            );
            var response = ResponseMapper.MapToCartResponseFromCart(cart);
            return Ok(ApiResponse<CartResponse>.OkResponse(response));
        }

        /// <summary>
        /// Update cart item quantity
        /// </summary>
        [HttpPut("items/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(Guid cartItemId, [FromBody] UpdateCartItemRequest request)
        {
            var cart = await _serviceProvider.CartService.HandleUpdateCartItemQuantityAsync(
                cartItemId,
                request.Quantity
            );
            var response = ResponseMapper.MapToCartResponseFromCart(cart);
            return Ok(ApiResponse<CartResponse>.OkResponse(response));
        }

        /// <summary>
        /// Remove a cart item
        /// </summary>
        [HttpDelete("items/{cartItemId}")]
        public async Task<IActionResult> RemoveCartItem(Guid cartItemId)
        {
            var cart = await _serviceProvider.CartService.HandleRemoveCartItemAsync(cartItemId);
            var response = ResponseMapper.MapToCartResponseFromCart(cart);
            return Ok(ApiResponse<CartResponse>.OkResponse(response));
        }

        /// <summary>
        /// Clear all items from cart
        /// </summary>
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var cart = await _serviceProvider.CartService.HandleClearCartAsync();
            var response = ResponseMapper.MapToCartResponseFromCart(cart);
            return Ok(ApiResponse<CartResponse>.OkResponse(response));
        }
    }
}
