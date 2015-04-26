using System.Collections.Generic;

namespace IglaClub.Web.Models.ViewModels
{
    public class TournamentListViewModel
    {
        public TournamentListViewModel(TournamentSingleListViewModel model)
        {
            this.TournamentsList = new List<TournamentSingleListViewModel>{model};
        }

        public TournamentListViewModel(IList<TournamentSingleListViewModel> tournamentsList)
        {
            TournamentsList = tournamentsList;
        }

        public IList<TournamentSingleListViewModel> TournamentsList { get; set; }
    }
}