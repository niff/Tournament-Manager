using System.Collections.Generic;
using IglaClub.ObjectModel.Entities;

namespace IglaClub.Web.Models.ViewModels.Clubs
{
    public class ClubsIndexAdminViewModel
    {
        public IList<Club> Clubs { get; set; }

        public ClubsIndexAdminViewModel(IList<Club> clubs)
        {
            Clubs = clubs;
        }
    }
}