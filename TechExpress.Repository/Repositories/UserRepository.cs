using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TechExpress.Repository.Contexts;
using TechExpress.Repository.Enums;
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

        public async Task<User?> FindUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> FindUserByPhoneAsync(string phone)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Phone == phone);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> FindUserByRoleAsync(UserRole role)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Role == role);
        }

        public async Task<bool> UserExistByRoleAsync(UserRole role)
        {
            return await _context.Users.AnyAsync(u => u.Role == role);
        }

        public async Task<bool> UserExistByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> UserExistByPhoneAsync(string phone)
        {
            return await _context.Users.AnyAsync(u => u.Phone == phone);
        }

        public async Task<User?> FindUserByEmailWithTrackingAsync(string email)
        {
            return await _context.Users.AsTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> UserExistsByIdAsync(Guid userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
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

        // ============= STAFF LIST =================
        public async Task<List<User>> GetStaffAsync(int page, int pageSize, StaffSortBy sortBy)
        {
            var query = _context.Users
                .Where(u => u.Role == UserRole.Staff);

            // == OrderBy tăng dần ==

            query = sortBy switch
            {
                StaffSortBy.Email => query.OrderBy(u => u.Email),
                StaffSortBy.FirstName => query.OrderBy(u => u.FirstName),
                StaffSortBy.Salary => query.OrderBy(u => u.Salary),
                _ => query.OrderBy(u => u.Email)
            };

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
