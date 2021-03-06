﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using IglaClub.ObjectModel.Enums;
using System.ComponentModel.DataAnnotations;

namespace IglaClub.ObjectModel.Entities
{
    public class Result :BaseEntity
    {
        public virtual Pair NS { get; set; }

        [Column("NS_Id")]
        public long? NSId { get; set; }
        
        public virtual Pair EW { get; set; }

        [Column("EW_Id")]
        public long? EWId { get; set; }

        [DisplayName ("Level")]
        [Range(0, 7, ErrorMessage = "Level must be between 1 and 7")]
        public int ContractLevel { get; set; }

        [DisplayName("Color")]
        public ContractColors ContractColor { get; set; }

        [DisplayName("Dbl")]
        public ContractDoubled ContractDoubled { get; set; }

        [DisplayName("Played By")]
        public PlayedBy PlayedBy { get; set; }

        [DisplayName("Tricks")]
        [Range(0, 13, ErrorMessage = "Number of tricks must be between 0 and 13")]
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

        public override string ToString()
        {
            var nsName = NS != null ? NS.ToString() :string.Empty;
            var ewName = EW != null ? EW.ToString() :string.Empty;
            return string.Format("Ns({2}): {0}, Ew({3}): {1}", ScoreNs, ScoreEw, nsName, ewName);
        }

        public bool UserIsInThisResult(long userId)
        {
            return (this.EW.Player1 != null && this.EW.Player1.Id == userId)
                    || (this.EW.Player2 != null && this.EW.Player2.Id == userId)
                    || (this.NS.Player1 != null && this.NS.Player1.Id == userId)
                    || (this.NS.Player2 != null && this.NS.Player2.Id == userId);
        }
        public bool UserIsNs(long userId)
        {
            return (this.NS.Player1 != null && this.NS.Player1.Id == userId)
                    || (this.NS.Player2 != null && this.NS.Player2.Id == userId);
        }

        public bool IsFinished
        {
            get
            {
                 return ScoreNs != null;    
            }
            //return this.ResultNsPoints != null || this.PlayedBy == Enums.PlayedBy.PassedOut || r.PlayedBy == Enums.PlayedBy.DirectorScore;
            
        }    

    }
}
