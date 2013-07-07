using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IglaClub.ObjectModel.Entities;
using IglaClub.ObjectModel.Enums;

namespace IglaClub.Web.Models.ViewModels
{
    public class PairsResultsViewModel
    {
        public TournamentScoringType TournamentScoringType { get; set; }
        public List<Pair> PairsInTounament { get; set; }
        public Dictionary<long, int> PairNumberMaxPoints { get; set; }
    }
    
}