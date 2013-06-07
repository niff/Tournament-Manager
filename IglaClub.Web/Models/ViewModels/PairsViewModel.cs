using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IglaClub.ObjectModel.Entities;

namespace IglaClub.Web.Models.ViewModels
{
    public class PairsViewModel
    {
        public List<Pair> PairsInTounament { get; set; }
        public List<User> AvailableUsers { get; set; }
    }
    
}