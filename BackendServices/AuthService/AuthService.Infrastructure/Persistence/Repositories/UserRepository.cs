using AuthService.Application.RepositoryContract;
using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDBContext _context;
        public UserRepository(AuthDBContext dBContext)
        {
            _context = dBContext;
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.
                Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Roles)
                .ToListAsync();
        }

        public async Task<bool> RegisterUser(User user, string role)
        {
            Role getRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == role);
            if (getRole != null)
            {
                user.Roles.Add(getRole);
                await _context.Users.AddAsync(user);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
