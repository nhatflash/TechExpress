using Microsoft.EntityFrameworkCore;
using TechExpress.Repository.Contexts;
using TechExpress.Repository.Models;

namespace TechExpress.Repository.Repositories
{
    public class ProductImageRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteByProductIdAsync(Guid productId)
        {
            var images = await _context.ProductImages
                .Where(x => x.ProductId == productId)
                .ToListAsync();

            if (images.Count == 0) return;

            _context.ProductImages.RemoveRange(images);
        }

        public async Task AddRangeAsync(List<ProductImage> images)
        {
            if (images == null || images.Count == 0) return;

            await _context.ProductImages.AddRangeAsync(images);
        }

        public async Task DeleteByIdsAsync(List<long> imageIds)
        {
            if (imageIds == null || imageIds.Count == 0) return;

            var images = await _context.ProductImages
                .Where(x => imageIds.Contains(x.Id))
                .ToListAsync();

            if (images.Count == 0) return;

            _context.ProductImages.RemoveRange(images);
        }

    }
}
