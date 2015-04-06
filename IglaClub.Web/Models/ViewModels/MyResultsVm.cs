using System.Collections.Generic;
using IglaClub.ObjectModel.Entities;

namespace IglaClub.Web.Models.ViewModels
{
    public class MyResultsVm
    {
        public List<Result> Results { get; set; }
        public Tournament Tournament { get; set; }
        public User CurrentUser { get; set; }

        public MyResultsVm(List<Result> results, Tournament tournament, User currentUser)
        {
            Results = results;
            Tournament = tournament;
            CurrentUser = currentUser;
        }
    }
}