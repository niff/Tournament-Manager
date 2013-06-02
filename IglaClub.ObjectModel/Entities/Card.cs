using IglaClub.ObjectModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IglaClub.ObjectModel.Entities
{
    public class Card : BaseEntity
    {
        public string Name { get; set; }

        public CardColor Color { get; set; }

        public string Value { get; set; }
    }
}
