using System.Collections.Generic;
using IglaClub.ObjectModel.Entities;

namespace IglaClub.Web.Models.ViewModels.Clubs
{
    public class ClubMembersViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public Club Club { get; set; }

        public ClubMembersViewModel(IEnumerable<User> users, Club club)
        {
            Users = users;
            Club = club;
        }
    }
}