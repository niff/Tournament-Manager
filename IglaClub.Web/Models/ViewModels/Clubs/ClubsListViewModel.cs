using System.Collections.Generic;
using IglaClub.ObjectModel.Entities;

namespace IglaClub.Web.Models.ViewModels.Clubs
{
    public class ClubsListViewModel
    {
        public IList<ClubViewModel> Clubs { get; set; }

        public User User { get; set; }

        public string Header { get; set; }

        public bool SubscribeMode { get; set; }
    }

    public class ClubViewModel
    {
        public Club Club { get; set; }
        public int MembersCount { get; set; }
    }
}