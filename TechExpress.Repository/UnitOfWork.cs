using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TechExpress.Repository.Contexts;
using TechExpress.Repository.Repositories;

namespace TechExpress.Repository
{
    public class UnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UserRepository UserRepository { get; }
        public CategoryRepository CategoryRepository { get; }

        public ProductRepository ProductRepository { get; } 
        public SpecDefinitionRepository SpecDefinitionRepository { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            UserRepository = new UserRepository(context);
            CategoryRepository = new CategoryRepository(context);
            ProductRepository = new ProductRepository(context);
            SpecDefinitionRepository = new SpecDefinitionRepository(context);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
