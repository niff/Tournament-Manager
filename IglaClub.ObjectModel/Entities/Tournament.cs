using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using IglaClub.ObjectModel.Enums;

namespace IglaClub.ObjectModel.Entities
{
    public class Tournament : BaseEntity
    {
        public Tournament()
        {
            //Pairs = new List<Pair>();
            //Boards = new List<BoardInstance>();
        }

        [Required]
        public string  Name { get; set; }

        public string Description { get; set; }

        public virtual Club Club { get; set; }

        public TournamentScoringType TournamentScoringType { get; set; }

        public TournamentMovingType TournamentMovingType { get; set; }

        public virtual IList<Pair> Pairs { get; set; }

        public int BoardsInRound { get; set; }

        public virtual IList<BoardInstance> Boards { get; set; }

        public virtual IList<Result> Results { get; set; }

        public TournamentStatus TournamentStatus { get; set; }

        public int CurrentRound { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? PlannedStartDate { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        


     }
}