using System.Collections;

namespace IglaClub.ObjectModel.Tools
{
    public static class ListExtensions
    {
        public static bool IsNullOrEmpty(this IList list)
        {
            return list == null || list.Count == 0;
        }
    }
}
