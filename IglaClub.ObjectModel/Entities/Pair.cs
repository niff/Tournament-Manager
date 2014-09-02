using System.ComponentModel;

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
            return (Player1 == null ? "Unknown" : Player1.Name) + " - " +(Player2 == null ? "Unknown" : Player2.Name);
        }

        
    }
}