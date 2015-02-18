using IglaClub.ObjectModel.Entities;

namespace IglaClub.Web.Models.ViewModels
{
    public class PairRosterViewModel
    {
        public int TableNumber { get; set; }
        public int Section { get; set; }
        public Pair NsPair { get; set; }
        public Pair EwPair { get; set; }
    }
    
}