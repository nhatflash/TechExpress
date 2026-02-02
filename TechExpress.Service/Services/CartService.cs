using System;
using System.Threading.Tasks;
using TechExpress.Repository;
using TechExpress.Repository.CustomExceptions;
using TechExpress.Repository.Enums;
using TechExpress.Repository.Models;
using TechExpress.Service.Contexts;

namespace TechExpress.Service.Services
{
    public class CartService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly UserContext _userContext;

        public CartService(UnitOfWork unitOfWork, UserContext userContext)
        {
            _unitOfWork = unitOfWork;
            _userContext = userContext;
        }

        public async Task<Cart> HandleGetCurrentCartAsync()
        {
            var userId = _userContext.GetCurrentAuthenticatedUserId();

            var cart = await _unitOfWork.CartRepository.FindActiveCartByUserIdNoTrackingAsync(userId);

            if (cart == null)
            {
                return new Cart
                {
                    Id = Guid.Empty,
                    UserId = userId,
                    Status = CartStatus.Active,
                    TotalPrice = 0,
                    Items = []
                };
            }

            return cart;
        }

        public async Task<Cart> HandleAddProductToCartAsync(Guid productId, int quantity)
        {
            if (quantity <= 0)
            {
                throw new BadRequestException("Số lượng phải lớn hơn 0.");
            }

            var userId = _userContext.GetCurrentAuthenticatedUserId();

            var product = await _unitOfWork.ProductRepository.FindByIdWithTrackingAsync(productId)
                ?? throw new NotFoundException("Không tìm thấy sản phẩm.");

            if (product.Status != ProductStatus.Available)
            {
                throw new BadRequestException("Sản phẩm hiện không khả dụng.");
            }

            var cart = await _unitOfWork.CartRepository.FindActiveCartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Status = CartStatus.Active,
                    TotalPrice = 0
                };
                await _unitOfWork.CartRepository.AddCartAsync(cart);
            }

            var existingItem = await _unitOfWork.CartRepository.FindCartItemByCartIdAndProductIdAsync(cart.Id, productId);

            if (existingItem != null)
            {
                var newQuantity = existingItem.Quantity + quantity;

                if (newQuantity > product.Stock)
                {
                    throw new BadRequestException($"Số lượng tồn kho không đủ. Chỉ còn {product.Stock} sản phẩm.");
                }

                existingItem.Quantity = newQuantity;
                existingItem.UnitPrice = product.Price;
                existingItem.UpdatedAt = DateTimeOffset.Now;
            }
            else
            {
                if (quantity > product.Stock)
                {
                    throw new BadRequestException($"Số lượng tồn kho không đủ. Chỉ còn {product.Stock} sản phẩm.");
                }

                var cartItem = new CartItem
                {
                    Id = Guid.NewGuid(),
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = product.Price
                };
                await _unitOfWork.CartRepository.AddCartItemAsync(cartItem);
                cart.Items.Add(cartItem);
            }

            cart.RecalculateTotalPrice();

            await _unitOfWork.SaveChangesAsync();

            return await _unitOfWork.CartRepository.FindActiveCartByUserIdNoTrackingAsync(userId) ?? cart;
        }

        public async Task<Cart> HandleUpdateCartItemQuantityAsync(Guid cartItemId, int quantity)
        {
            if (quantity < 0)
            {
                throw new BadRequestException("Số lượng không được âm.");
            }

            var userId = _userContext.GetCurrentAuthenticatedUserId();

            var cart = await _unitOfWork.CartRepository.FindActiveCartByUserIdAsync(userId)
                ?? throw new NotFoundException("Không tìm thấy giỏ hàng.");

            var cartItem = await _unitOfWork.CartRepository.FindCartItemByIdAsync(cartItemId)
                ?? throw new NotFoundException("Không tìm thấy sản phẩm trong giỏ hàng.");

            if (cartItem.CartId != cart.Id)
            {
                throw new ForbiddenException("Bạn không có quyền cập nhật sản phẩm này.");
            }

            if (quantity == 0)
            {
                _unitOfWork.CartRepository.RemoveCartItem(cartItem);
                cart.Items.Remove(cartItem);
            }
            else
            {
                var product = cartItem.Product
                    ?? await _unitOfWork.ProductRepository.FindByIdWithTrackingAsync(cartItem.ProductId)
                    ?? throw new NotFoundException("Không tìm thấy sản phẩm.");

                if (product.Status != ProductStatus.Available)
                {
                    throw new BadRequestException("Sản phẩm hiện không khả dụng.");
                }

                if (quantity > product.Stock)
                {
                    throw new BadRequestException($"Số lượng tồn kho không đủ. Chỉ còn  {product.Stock}  sản phẩm.");
                }

                cartItem.Quantity = quantity;
                cartItem.UnitPrice = product.Price;
                cartItem.UpdatedAt = DateTimeOffset.Now;
            }

            cart.RecalculateTotalPrice();

            await _unitOfWork.SaveChangesAsync();

            return await _unitOfWork.CartRepository.FindActiveCartByUserIdNoTrackingAsync(userId) ?? cart;
        }

        public async Task<Cart> HandleRemoveCartItemAsync(Guid cartItemId)
        {
            var userId = _userContext.GetCurrentAuthenticatedUserId();

            var cart = await _unitOfWork.CartRepository.FindActiveCartByUserIdAsync(userId)
                ?? throw new NotFoundException("Không tìm thấy giỏ hàng.");

            var cartItem = await _unitOfWork.CartRepository.FindCartItemByIdAsync(cartItemId)
                ?? throw new NotFoundException("Không tìm thấy sản phẩm trong giỏ hàng.");

            if (cartItem.CartId != cart.Id)
            {
                throw new ForbiddenException("Bạn không có quyền xoá sản phẩm này.");
            }

            _unitOfWork.CartRepository.RemoveCartItem(cartItem);
            cart.Items.Remove(cartItem);

            cart.RecalculateTotalPrice();

            await _unitOfWork.SaveChangesAsync();

            return await _unitOfWork.CartRepository.FindActiveCartByUserIdNoTrackingAsync(userId) ?? cart;
        }

        public async Task<Cart> HandleClearCartAsync()
        {
            var userId = _userContext.GetCurrentAuthenticatedUserId();

            var cart = await _unitOfWork.CartRepository.FindActiveCartByUserIdAsync(userId)
                ?? throw new NotFoundException("Không tìm thấy giỏ hàng.");

            _unitOfWork.CartRepository.ClearCartItems(cart);
            cart.Items.Clear();
            cart.TotalPrice = 0;
            cart.UpdatedAt = DateTimeOffset.Now;

            await _unitOfWork.SaveChangesAsync();

            return cart;
        }
    }
}
