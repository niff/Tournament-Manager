using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IglaClub.ObjectModel.Enums;

namespace IglaClub.ObjectModel.Entities
{
    public class BoardDefinition : BaseEntity
    {
        public PlayedBy Dealer { get; set; }

        public Vulnerable Vulnerability { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual IList<Card> North { get; set; }
        public virtual IList<Card> East { get; set; }
        public virtual IList<Card> South { get; set; }
        public virtual IList<Card> West { get; set; }
    }

}