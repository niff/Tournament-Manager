using System.Collections.Generic;
using IglaClub.ObjectModel.Entities;

namespace IglaClub.Web.Models.ViewModels.Clubs
{
    public class ClubMembersViewModel
    {
        public IList<ClubUser> ClubUsers { get; set; }
        public Club Club { get; set; }

        public ClubMembersViewModel(IList<ClubUser> users, Club club)
        {
            ClubUsers = users;
            Club = club;
        }
    }
}