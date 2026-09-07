using BookWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWeb.Business.Services.IServices
{
    public interface IOrderService
    {
        Task<OrderHeader> CreateOrderAsync(OrderHeader orderHeader);
        Task<OrderHeader?> GetOrderByIdAsync(int id , bool includeUser = false , bool includeDetails = false );
        Task<IEnumerable<OrderHeader>> GetAllOrderAsync(string? userId = null, string? status = null,bool includeUser = false , bool includeDetails = false );
        Task UpadateOrderAsync(OrderHeader orderHeader);
        Task UpadateOrderStatusAsync(int id , string orderStatus, string? carrier = null, string? trackingNumber = null);
    }
}
