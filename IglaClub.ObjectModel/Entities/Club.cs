using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IglaClub.ObjectModel.Entities
{
    public class Club : BaseEntity
    {
        public string  Name { get; set; }

        public string Description { get; set; }

        public virtual IList<User> Users { get; set; }
    }
}