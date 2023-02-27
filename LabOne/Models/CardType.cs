using System.ComponentModel.DataAnnotations;

namespace LabOne.Models;

public enum CardType
{
    Visa,
    
    [Display(Name = "Master Card")]
    MasterCard,
    
    Discover
}