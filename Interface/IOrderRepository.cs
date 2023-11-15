using Microsoft.AspNetCore.Mvc;
using MobFDB.Models;

namespace MobFDB.Interface
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrders();
        Task<Order> GetOrder(int id);
        Task PutOrder(Order order);
        Task<Order> PostOrder(Order order);
        Task DeleteOrder(int id);
       
        bool OrderExists(int id);
    }

}
