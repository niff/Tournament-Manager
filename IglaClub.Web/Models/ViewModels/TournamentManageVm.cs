using IglaClub.ObjectModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IglaClub.Web.Models.ViewModels
{
    public class TournamentManageVm
    {
        public Tournament Tournament { get; set; }
        //public IList<User> NotAssignedUsersFromClub { get; set; }
        public int ResultsToComplete
        {
            get { return Tournament.Results.Count(r => r.ResultNsPoints == null && r.RoundNumber == Tournament.CurrentRound); }
        }
    }
}