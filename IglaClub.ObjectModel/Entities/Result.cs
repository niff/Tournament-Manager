using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using IglaClub.ObjectModel.Enums;

namespace IglaClub.ObjectModel.Entities
{
    public class Result :BaseEntity
    {
        public virtual Pair NS { get; set; }

        public virtual Pair EW { get; set; }

        [DisplayName ("Level")]
        public int ContractLevel { get; set; }

        [DisplayName("Color")]
        public ContractColors ContractColor { get; set; }

        [DisplayName("Dbl")]
        public ContractDoubled ContractDoubled { get; set; }

        [DisplayName("Played By")]
        public NESW PlayedBy { get; set; }

        [DisplayName("Tricks")]
        public int NumberOfTricks { get; set; }

        [DisplayName("Ns Score")]
        public int? ResultNsPoints { get; set; }

        public virtual BoardInstance Board { get; set; }

        [Column("Board_Id")]
        public long BoardId { get; set; }

        public virtual Tournament Tournament { get; set; }

        [Column("Tournament_Id")]
        public long TournamentId { get; set; }

        [DisplayName("Round")]
        public int RoundNumber { get; set; }

        [DisplayName("Table")]
        public int TableNumber { get; set; }

        [DisplayName("Ns")]
        public float? ScoreNs { get; set; }

        [DisplayName("Ew")]
        public float? ScoreEw { get; set; }
    }
}
