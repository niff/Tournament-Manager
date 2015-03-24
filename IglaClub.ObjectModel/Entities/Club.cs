using System.Collections.Generic;

namespace IglaClub.ObjectModel.Entities
{
    public class Club : BaseEntity
    {
        public string  Name { get; set; }

        public string Description { get; set; }

        public virtual IList<ClubUser> ClubUsers { get; set; }

        public string Coordinates { get; set; }

        public string Address { get; set; }
    }
}