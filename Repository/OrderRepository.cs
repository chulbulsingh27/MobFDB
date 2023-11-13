using Microsoft.EntityFrameworkCore;
using MobFDB.Interface;
using MobFDB.Models;

namespace MobFDB.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MobDbContext _context;

        public OrderRepository(MobDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Product)
                .ToListAsync();
        }

        public async Task<Order> GetOrder(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task PutOrder(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Order> PostOrder(Order order)
        {
            // Load the related User and Product entities
            var user = await _context.Users.FindAsync(order.UserId);
            var product = await _context.Products.FindAsync(order.ProductId);

            if (user == null || product == null)
            {
                throw new Exception("User or Product not found");
            }

          
            order.User = user;
            order.Product = product;

            
            string discountString = product.OffersAndDiscounts.TrimEnd('%');
            if (decimal.TryParse(discountString, out decimal discount))
            {
                discount /= 100;  // Convert the discount from a percentage to a decimal
                order.TotalAmount = product.Price * (1 - discount) * order.Quantity;
            }
            else
            {
                throw new Exception("Invalid discount format");
            }
            if (order.OrderDate.HasValue)
            {
               
                order.Product.DeliveryDate = order.OrderDate.Value.AddDays(2);
            }
            else
            {
                throw new Exception("OrderDate is null");
            }

            /*order.Product.DeliveryDate = order.OrderDate.AddDays(2);*/

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }

}
