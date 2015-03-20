using System.Collections.Generic;
using System.Device.Location;

namespace IglaClub.ObjectModel.Entities
{
    public class Club : BaseEntity
    {
        public string  Name { get; set; }

        public string Description { get; set; }

        public virtual IList<User> Users { get; set; }

        public string Coordinates { get; set; }

        public string Address { get; set; }
    }
}