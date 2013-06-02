using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IglaClub.ObjectModel.Entities
{
    public class User : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Lastname { get; set; }

        [MaxLength(100)]
        public string Nickname { get; set; }

        [MaxLength(100)]
        public string Login { get; set; }

        public string Email { get; set; }

        public virtual IList<Club> Clubs { get; set; }

    }
}