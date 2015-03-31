using System;

namespace IglaClub.ObjectModel.Enums
{
    [Flags]
    public enum ContractColors
    {
        Unknown = 0,
        Spade = 1,
        Heart = 2,
        Diamond = 4,
        Club = 8,
        NoTrump = 16
    }
}