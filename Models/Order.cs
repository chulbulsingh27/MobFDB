using System.ComponentModel.DataAnnotations;

namespace MobFDB.Models
{
    public partial class Order
    {

        [Key]
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public int? ProductId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? Quantity { get; set; }

        public virtual Product? Product { get; set; }
        public virtual User? User { get; set; }
    }
}
