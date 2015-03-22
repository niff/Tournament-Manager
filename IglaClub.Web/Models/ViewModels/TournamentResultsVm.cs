using IglaClub.ObjectModel.Entities;
using System.Collections.Generic;

namespace IglaClub.Web.Models.ViewModels
{
    public class TournamentResultsVm
    {
        public Tournament Tournament { get; set; }

        public IList<Result> Results { get; set; }
        
        public string SortBy { get; set; }

        public string SortOrder { get; set; }
    }
}