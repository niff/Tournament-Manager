using System.ComponentModel.DataAnnotations;

namespace IglaClub.ObjectModel.Enums
{

    public enum ContractDoubled
    {
        [Display(Name = "")]
        NotDoubled = 0,
        [Display(Name = "X")]
        Doubled = 2,
        [Display(Name = "XX")]
        Redoubled = 4 
    }
}