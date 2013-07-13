using System.ComponentModel.DataAnnotations.Schema;
using IglaClub.ObjectModel.Enums;

namespace IglaClub.ObjectModel.Entities
{
    public class Result :BaseEntity
    {
        public virtual Pair NS { get; set; }

        public virtual Pair EW { get; set; }

        public int ContractLevel { get; set; }

        public ContractColors ContractColor { get; set; }
        
        public ContractDoubled ContractDoubled { get; set; }

        public NESW PlayedBy { get; set; }
        
        public int NumberOfTricks { get; set; }

        public int? ResultNsPoints { get; set; }


        public virtual BoardInstance Board { get; set; }

        [Column("Board_Id")]
        public long BoardId { get; set; }

        public virtual Tournament Tournament { get; set; }

        [Column("Tournament_Id")]
        public long TournamentId { get; set; }

        public int RoundNumber { get; set; }

        public int TableNumber { get; set; }

        public float? ScoreNs { get; set; }
        public float? ScoreEw { get; set; }
    }
}
