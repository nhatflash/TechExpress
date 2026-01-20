using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using TechExpress.Repository.Contexts;
using TechExpress.Repository.Models;

namespace TechExpress.Repository.Repositories
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> FindUserByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> FindUserByIdWithTrackingAsync(Guid id)
        {
            return await _context.Users.AsTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> FindUserByEmailAsync(string emailNorm)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == emailNorm);
        }

        public async Task<User?> FindUserByEmailWithTrackingAsync(string emailNorm)
        {
            return await _context.Users.AsTracking()
                .FirstOrDefaultAsync(u => u.Email.ToLower() == emailNorm);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

    }
}
