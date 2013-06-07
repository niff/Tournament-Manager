using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IglaClub.ObjectModel.Repositories
{
    

    public class BaseRepository
    {
        public readonly IIglaClubDbContext db;

        public BaseRepository(IIglaClubDbContext _db)
        {
            db = _db;
        }
    }
}
