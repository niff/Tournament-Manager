using System.Collections.Generic;
using IglaClub.ObjectModel.Entities;

namespace IglaClub.Web.Models.ViewModels
{
    public class TounamentSingleListViewModel
    {
        public IList<Tournament> Tournaments { get; set; }
        public IList<Tournament> TournamentsPast { get; set; }
        public string Header { get; set; }
        public bool ManageMode { get; set; }
        public bool ShowSubscriptionStatus { get; set; }
    }
}