using System.Collections.Generic;
using System.Linq;

namespace IglaClub.ObjectModel.Entities
{
    public class Club : BaseEntity
    {
        public string  Name { get; set; }

        public string Description { get; set; }

        public virtual IList<ClubUser> ClubUsers { get; set; }

        public string Coordinates { get; set; }

        public string Address { get; set; }

        public bool UserIsSubscribed(long userId)
        {
            return this.ClubUsers.Any(cu => cu.User.Id == userId);
        }
    }
}