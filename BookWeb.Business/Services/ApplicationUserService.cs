using BookWeb.Business.Services.IServices;
using BookWeb.DataAccess.Data;
using BookWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWeb.Business.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly ApplicationDbContext _context;
        public ApplicationUserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.ApplicationUsers.ToListAsync();
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            return await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == userId);
        }

    }
}
