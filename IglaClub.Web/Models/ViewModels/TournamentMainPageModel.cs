using System.Collections.Generic;
using IglaClub.ObjectModel.Entities;

namespace IglaClub.Web.Models.ViewModels
{
    public class TournamentMainPageModel
    {
        public IList<Tournament> CurrentlyPlayedByUser { get; set; }

        public bool UserIsManagingTournament { get; set; }
    }
}