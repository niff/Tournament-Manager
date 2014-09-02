using IglaClub.ObjectModel.Entities;
using System.Linq;

namespace IglaClub.Web.Models.ViewModels
{
    public class TournamentManageVm
    {
        public Tournament Tournament { get; set; }

        public int ResultsToComplete
        {
            get { return Tournament.Results.Count(r => r.ResultNsPoints == null && r.RoundNumber == Tournament.CurrentRound); }
        }
    }
}