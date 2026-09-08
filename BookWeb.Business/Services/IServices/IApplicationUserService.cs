using BookWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWeb.Business.Services.IServices
{
    public interface IApplicationUserService
    {
        Task<ApplicationUser?> GetUserByIdAsync(string userId); 
        Task <IEnumerable<ApplicationUser>> GetAllUsersAsync(); 
    }
}
