using System;

namespace IglaClub.ObjectModel.Enums
{
    [Flags]
    public enum CardColor
    {
        Spade = 1,
        Heart = 2,
        Diamond = 4,
        Club = 8
    }
}