using System.Collections.Generic;
using IglaClub.ObjectModel.Entities;
using IglaClub.ObjectModel.Enums;

namespace IglaClub.Web.Models.ViewModels
{
    public class PairsResultsViewModel
    {
        public TournamentScoringType TournamentScoringType { get; set; }
        public List<Pair> PairsInTounament { get; set; }
        public Dictionary<long, int> PairNumberMaxPoints { get; set; }
        public long TournamentId { get; set; }
        public string CurrentUserLogin { get; set; }
    }
    
}