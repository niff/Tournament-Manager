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

        public virtual void InsertOrUpdate(BaseEntity entity)
        {
                //db.Entry(entity).State = entity.Id == 0 ?
                //                           EntityState.Added :
                //                           EntityState.Modified;
            var dbObj = db.Results.Find(entity.Id);
            db.Entry(dbObj).CurrentValues.SetValues(entity);
            db.SaveChanges();
        }

        //public void InsertOrUpdate<T>(T entity) where T: BaseEntity
        //{
        //    //db.Entry(entity).State = entity.Id == 0 ?
        //    //                           EntityState.Added :
        //    //                           EntityState.Modified;
            
        //    var dbObj = db..Find(entity.Id);
        //    db.Entry(dbObj).CurrentValues.SetValues(entity);
        //    db.SaveChanges();
        //}

        //public T Get<T>(long id)
        //{
        //    (new IglaClubDbContext()).
        //    db.
        //}
    }
}
