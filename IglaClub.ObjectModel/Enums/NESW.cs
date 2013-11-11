using System;

namespace IglaClub.ObjectModel.Enums
{
    [Flags]
    public enum NESW
    {
        Unavailable = 1,
        North = 2,
        East = 4,
        South = 8,
        West = 16,
        PassedOut = 32,
        DirectorScore = 64
    }
}