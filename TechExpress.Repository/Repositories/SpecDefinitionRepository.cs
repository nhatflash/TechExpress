using System;
using System.Collections.Generic;
using System.Text;
using TechExpress.Repository.Contexts;
using TechExpress.Repository.Models;
using Microsoft.EntityFrameworkCore;


namespace TechExpress.Repository.Repositories
{
    public class SpecDefinitionRepository
    {
        private readonly ApplicationDbContext _context;

        public SpecDefinitionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SpecDefinition>> GetByCategoryIdAsync(Guid categoryId)
        {
            return await _context.SpecDefinitions
                .Where(s => s.CategoryId == categoryId && !s.IsDeleted)
                .ToListAsync();
        }

       
    }
}
