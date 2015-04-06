using System.Collections.Generic;
using IglaClub.ObjectModel.Entities;

namespace IglaClub.Web.Models.ViewModels.Clubs
{
    public class ClubsIndexViewModel
    {
        public IList<Club> ClubsWithSubscribedUser { get; set; }
        public IList<Club> ClubsWithNotSubscribedUser { get; set; }
        public User User { get; set; }
    }
}