using System;

namespace IglaClub.ObjectModel.Enums
{
    [Flags]
    public enum NESW
    {
        North = 1,
        East = 2,
        South = 4,
        West = 8,
        PassedOut = 16
    }
}