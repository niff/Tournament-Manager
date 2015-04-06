using System.Collections.Generic;
using IglaClub.ObjectModel.Entities;

namespace IglaClub.Web.Models.ViewModels.Clubs
{
    public class ClubsListViewModel
    {
        public IList<Club> Clubs { get; set; }
        public User User { get; set; }

        public string Header { get; set; }
    }
}