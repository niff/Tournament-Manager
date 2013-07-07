using IglaClub.ObjectModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IglaClub.Web.Models.ViewModels
{
    public class TournamentResultsVm
    {
        public Tournament Tournament { get; set; }

        public IList<Result> Results { get; set; }

    }
}