using System;
using Microsoft.EntityFrameworkCore;
using TechExpress.Repository.Contexts;
using TechExpress.Repository.Models;

namespace TechExpress.Repository.Repositories;

public class CartItemRepository
{
    private readonly ApplicationDbContext _context;

    public CartItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CartItem?> FindCartItemByIdIncludeProductWithTrackingAsync(Guid cartItemId)
    {
        return await _context.CartItems
            .AsTracking()
            .Include(ci => ci.Product)
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId);
    }

    public async Task<CartItem?> FindCartItemByIdWithTrackingAsync(Guid cartItemId)
    {
        return await _context.CartItems
            .AsTracking()
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId);
    }


    public async Task<CartItem?> FindCartItemByCartIdAndProductIdWithTrackingAsync(Guid cartId, Guid productId)
    {
        return await _context.CartItems
            .AsTracking()
            .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
    }

    public async Task AddCartItemAsync(CartItem cartItem)
    {
        await _context.CartItems.AddAsync(cartItem);
    }

    public void RemoveCartItem(CartItem cartItem)
    {
        _context.CartItems.Remove(cartItem);
    }

    public void ClearCartItems(List<CartItem> cartItems)
    {
        _context.CartItems.RemoveRange(cartItems);
    }
}
