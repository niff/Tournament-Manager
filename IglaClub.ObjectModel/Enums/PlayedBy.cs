using System;
using System.ComponentModel.DataAnnotations;

namespace IglaClub.ObjectModel.Enums
{
    [Flags]
    public enum PlayedBy
    {
        [Display(Name = "n/a")]
        Unavailable = 0,
        [Display(Name = "N")]
        North = 2,
        [Display(Name = "E")]
        East = 4,
        [Display(Name = "S")]
        South = 8,
        [Display(Name = "W")]
        West = 16,
        [Display(Name = "p/o")]
        PassedOut = 32,
        [Display(Name = "d/s")]
        DirectorScore = 64
    }
}