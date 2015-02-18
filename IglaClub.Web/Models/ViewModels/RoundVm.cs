using System.Collections.Generic;
using IglaClub.ObjectModel.Entities;

namespace IglaClub.Web.Models.ViewModels
{
    public class RoundVm
    {
        public List<Result> Results { get; set; }
        public Pair NsPair { get; set; }
        public Pair EwPair { get; set; }
    }
}