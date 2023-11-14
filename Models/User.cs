
using System.ComponentModel.DataAnnotations;

namespace MobFDB.Models
{
    public partial class User
    {
        /*  public User()
          {
              Orders = new HashSet<Order>();
          }*/

        [Key]
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
       /* [RegularExpression(@"^\d+$", ErrorMessage = "MobileNumber must contain only numeric characters.")]*/
        public string MobileNumber { get; set; }
        [Required]
        public string Password { get; set; }
        

        public string? Role { get; set; }

       /* public DateTime? RegistrationDate { get; set; }*/
       /* public int? BonusPoints { get; set; }*/

        /*  public virtual ICollection<Order> Orders { get; set; }*/

    }
}
