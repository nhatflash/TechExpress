using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TechExpress.Repository.Contexts;
using TechExpress.Repository.Enums;
using TechExpress.Repository.Models;

namespace TechExpress.Repository.Repositories
{
    public class CartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart?> FindActiveCartByUserIdAsync(Guid userId)
        {
            return await _context.Carts
                .AsTracking()
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == CartStatus.Active);
        }

        public async Task<Cart?> FindActiveCartByUserIdNoTrackingAsync(Guid userId)
        {
            return await _context.Carts
                .AsNoTracking()
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == CartStatus.Active);
        }

        public async Task<Cart?> FindCartByIdAsync(Guid cartId)
        {
            return await _context.Carts
                .AsTracking()
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(c => c.Id == cartId);
        }

        public async Task AddCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
        }

        public async Task<CartItem?> FindCartItemByCartIdAndProductIdAsync(Guid cartId, Guid productId)
        {
            return await _context.CartItems
                .AsTracking()
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }

        public async Task<CartItem?> FindCartItemByIdAsync(Guid cartItemId)
        {
            return await _context.CartItems
                .AsTracking()
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);
        }

        public async Task AddCartItemAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
        }

        public void RemoveCartItem(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
        }

        public void ClearCartItems(Cart cart)
        {
            _context.CartItems.RemoveRange(cart.Items);
        }
    }
}
