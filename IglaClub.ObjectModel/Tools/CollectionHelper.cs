using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IglaClub.ObjectModel.Tools
{
    static class CollectionHelper
    {
        public static void AddRange<T>(this IList<T> orgList, IEnumerable<T> listToAdd)
        {
            if(Equals(orgList, listToAdd))
                return;
            if (listToAdd == null)
                throw new ArgumentNullException("listToAdd");
            foreach (var item in listToAdd)
            {
                orgList.Add(item);
            }
        }
    }
}
