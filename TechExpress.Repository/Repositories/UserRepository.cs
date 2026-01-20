using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<(List<User> Users, int TotalCount)> GetUsersPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Users.AsNoTracking();

            var totalCount = await query.CountAsync();

            var users = await query
                .OrderBy(u => u.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (users, totalCount);
        }

        public async Task<User?> GetUserDetailByIdAsync(Guid id)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> FindUserByEmailAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            return user;
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
        }
    }
}
