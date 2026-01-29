using Microsoft.EntityFrameworkCore;
using TechExpress.Repository.Contexts;
using TechExpress.Repository.Models;

namespace TechExpress.Repository.Repositories
{
    public class ProductSpecValueRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductSpecValueRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductSpecValue>> GetByProductIdWithTrackingAsync(Guid productId)
        {
            return await _context.ProductSpecValues
                .AsTracking()
                .Where(x => x.ProductId == productId)
                .ToListAsync();
        }

        public async Task AddAsync(ProductSpecValue entity)
        {
            await _context.ProductSpecValues.AddAsync(entity);
        }
    }
}
