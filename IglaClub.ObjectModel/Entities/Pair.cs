using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IglaClub.ObjectModel.Entities
{
    public class Pair : BaseEntity
    {
        //public List<User> Users { get; set; }

        public virtual Tournament Tournament { get; set; }

        public int PairNumber { get; set; }

        public virtual User Player1 { get; set; }

        public virtual User Player2 { get; set; }
    }
}