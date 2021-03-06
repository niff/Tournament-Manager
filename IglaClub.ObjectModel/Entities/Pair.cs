﻿using System.ComponentModel;

namespace IglaClub.ObjectModel.Entities
{
    public class Pair : BaseEntity
    {
        //public List<User> Users { get; set; }

        public virtual Tournament Tournament { get; set; }

        [DisplayName("Pair Number")]
        public int PairNumber { get; set; }

        [DisplayName("Player 1")]
        public virtual User Player1 { get; set; }

        [DisplayName("Player 2")]
        public virtual User Player2 { get; set; }

        public float Score { get; set; }

        public override string ToString()
        {
            if (Player1 == null && Player2 == null)
                return "BYE";
            return (Player1 == null ? "Unknown" : Player1.GetDisplayName()) + " - " + (Player2 == null ? "Unknown" : Player2.GetDisplayName()) + "["+this.PairNumber+"]";
        }

        public bool IsByePair
        {
            get
            {
                return (Player1 == null && Player2 == null);
            } 
        }

        public bool ContainsUser(long userId)
        {
            return (this.Player1 != null && this.Player1.Id == userId) ||
                   (this.Player2 != null && this.Player2.Id == userId);
        }
    }
}