using IglaClub.ObjectModel.Enums;

namespace IglaClub.ObjectModel.Entities
{
    public class Result :BaseEntity
    {
        public virtual Pair NS { get; set; }

        public virtual Pair EW { get; set; }

        public NESW PlayedBy { get; set; }
        
        public int NumberOfTricks { get; set; }
        
        public ContractColors ContractColor { get; set; }
        
        public ContractDoubled ContractDoubled { get; set; }

        public int ContractLevel { get; set; }

        public int? ResultNsPoints { get; set; }

        public virtual BoardInstance Board { get; set; }

        public virtual Tournament Tournament { get; set; }

        public int RoundNumber { get; set; }

        public int TableNumber { get; set; }

        public float? ScoreNs { get; set; }
        public float? ScoreEw { get; set; }
    }
}
