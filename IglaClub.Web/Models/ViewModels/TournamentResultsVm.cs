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

        public IList<ResultVm> ResultsVm
        {
            get 
            {
                return Tournament.Results.Select(r => new
                                                          ResultVm()
                    {
                        //NsId = r.NS.Id,
                        //EwId = r.EW.Id,
                        EwPairNumber = r.EW.PairNumber,
                        NsPairNumber = r.NS.PairNumber,
                        NsName = r.NS.ToString(),
                        EwName = r.EW.ToString(),
                        RoundNumber = r.RoundNumber,
                        TableNumber = r.TableNumber,
                        Board = r.Board.Id
                    }).ToList();
            }
        }

        public class ResultVm
        {
            //public long NsId { get; set; }
            //public long EwId { get; set; }
            public long NsPairNumber { get; set; }
            public long EwPairNumber { get; set; }
            public int RoundNumber { get; set; }
            public int TableNumber { get; set; }

            public long Board{ get; set; }

            public string EwName { get; set; }
            public string NsName { get; set; }

            
            
            
        }
        //public IList<User> NotAssignedUsersFromClub { get; set; }
    }
}