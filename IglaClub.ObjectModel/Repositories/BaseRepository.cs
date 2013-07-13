using IglaClub.ObjectModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Data;

namespace IglaClub.ObjectModel.Repositories
{
    

    public class BaseRepository
    {
        public readonly IIglaClubDbContext db;

        public BaseRepository(IIglaClubDbContext _db)
        {
            db = _db;
        }

        public void InsertOrUpdate(BaseEntity entity)
        {
                db.Entry(entity).State = entity.Id == 0 ?
                                           EntityState.Added :
                                           EntityState.Modified;
                db.SaveChanges();
        }

        //public T Get<T>(long id)
        //{
        //    (new IglaClubDbContext()).
        //    db.
        //}
    }
}
