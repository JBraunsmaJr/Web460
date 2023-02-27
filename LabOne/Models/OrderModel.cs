using System.ComponentModel.DataAnnotations;

namespace LabOne.Models;

public class OrderModel
{
    public bool IsReadOnly { get; set; }

    [Required, Display(Name="First Name")]
    public string FirstName { get; set; }
    
    [Required, Display(Name="Last Name")]
    public string LastName { get; set; }

    [Required]
    public string Street { get; set; }
    
    [Required]
    public string City { get; set; }
    
    [Required]
    public string State { get; set; }

    /// <remarks>
    ///     <para>
    ///         Do note that the credit card attribute uses the Luhn check. During testing you may find that
    ///         certain 16 digit combinations are considered invalid.
    ///     </para>
    ///     <para>
    ///         For giggles, 1234-1234-1234-1234-1234 works.
    ///     </para>
    /// </remarks>
    [Required, CreditCard, Display(Name = "Credit Card Number")]
    public string CreditCardNumber { get; set; }

    [Display(Name = "Card Type")]
    public CardType CardType { get; set; }

    [Required, Phone, Display(Name="Phone Number")]
    public string PhoneNumber { get; set; }
}