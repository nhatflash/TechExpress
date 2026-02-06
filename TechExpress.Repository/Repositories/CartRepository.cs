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

        public async Task<Cart?> FindCartByUserIdIncludeItemsThenIncludeProductThenIncludeImagesWithTrackingAndSplitQueryAsync(Guid userId)
        {
            return await _context.Carts
                .AsTracking()
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.Images)
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart?> FindCartByUserIdIncludeItemsThenIncludeProductThenIncludeImagesWithTrackingAsync(Guid userId)
        {
            return await _context.Carts
                .AsTracking()
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart?> FindCartByUserIdIncludeItemsThenIncludeProductThenIncludeImagesWithSplitQueryAsync(Guid userId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.Images)
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart?> FindCartByUserIdIncludeItemsThenIncludeProductThenIncludeImagesAsync(Guid userId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart?> FindCartByUserIdAsync(Guid userId)
        {
            return await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart?> FindCartByUserIdWithTrackingAsync(Guid userId)
        {
            return await _context.Carts.AsTracking().FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart?> FindCartByUserIdIncludeItemsWithTrackingAndSplitQueryAsync(Guid userId)
        {
            return await _context.Carts.AsTracking().Include(c => c.Items).AsSplitQuery().FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart?> FindCartByUserIdIncludeItemsWithTrackingAsync(Guid userId)
        {
            return await _context.Carts.AsTracking().Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart?> FindCartByIdIncludeItemsThenIncludeProductThenIncludeImagesWithtrackingAsync(Guid cartId)
        {
            return await _context.Carts
                .AsTracking()
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(c => c.Id == cartId);
        }

        public async Task<Cart?> FindCartByIdAsync(Guid cartId)
        {
            return await _context.Carts.FirstOrDefaultAsync(c => c.Id == cartId);
        }

        public async Task AddCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
        }

    }
}
