using System.ComponentModel.DataAnnotations;
using Database.Attributes;

namespace BraunsmaWeek3.Models
{
    [Table(Name = "CustomerOrders")]
    public class Order
    {
        [Id]
        public int Id { get; set; }

        [MaxLength(50), Required]
        public string FirstName { get; set; }
        
        [MaxLength(50), Required]
        public string LastName { get; set; }

        [MaxLength(75), Required]
        public string Street { get; set; }

        [MaxLength(75), Required]
        public string City { get; set; }

        [MaxLength(75), Required]
        public string State { get; set; }
        
        [MaxLength(14), Required]
        public string PhoneNumber { get; set; }

        [MaxLength(16), Required]
        public string PaymentType { get; set; }
        
        [MaxLength(24), Required]
        public string CreditCardNumber { get; set; }
    }
}