using System.ComponentModel.DataAnnotations;

namespace MobFDB.Models
{
    public partial class Product
    {
        /* public Product()
         {
             Orders = new HashSet<Order>();
         }*/
        [Key]
        public int ProductId { get; set; }
        public string? ModelName { get; set; }
        public decimal? Price { get; set; }
        public string? OffersAndDiscounts { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? BillingMethod { get; set; }
        public byte? CustomerCareSupport { get; set; }
        public byte? CancelAndReturnPolicy { get; set; }

        /*  public virtual ICollection<Order> Orders { get; set; }*/
    }
}
