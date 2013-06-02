﻿using IglaClub.ObjectModel.Enums;

namespace IglaClub.ObjectModel.Entities
{
    public class Result :BaseEntity
    {

        public virtual Pair NS { get; set; }

        public virtual Pair EW { get; set; }

        public NESW PlayedBy { get; set; }
        
        public int NumberOfTricks { get; set; }
        
        public ContractColors ContractColor { get; set; }

        public int ContractLevel { get; set; }
    }
}
