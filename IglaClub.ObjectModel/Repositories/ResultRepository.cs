using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IglaClub.ObjectModel.Entities;

namespace IglaClub.ObjectModel.Repositories
{
    class ResultRepository :BaseRepository
    {
        public ResultRepository(IIglaClubDbContext dbContext)
            : base(dbContext)
        {
        }

        public List<Result> GetResults(long tournamentId, int roundNumber, int tableNumber)
        {
            throw new NotImplementedException();
        }
    }
}
