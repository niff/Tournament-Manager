using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using IglaClub.ObjectModel.Entities;

namespace IglaClub.ObjectModel.Repositories
{
    

    public class BaseRepository
    {
        public readonly IIglaClubDbContext db;

        public BaseRepository(IIglaClubDbContext _db)
        {
            db = _db;
        }

        public virtual T InsertOrUpdate<T>(T entity) where T: BaseEntity
        {
            T updatedEntity = null;
            if (entity.Id > 0)
            {
                var targetDbSet = GetDbSet<T>();
                var dbObj = targetDbSet.Find(entity.Id);
                db.Entry(dbObj).CurrentValues.SetValues(entity);
                updatedEntity = dbObj;
            }
            else
            {
                var targetDbSet = GetDbSet<T>();
                updatedEntity = targetDbSet.Add(entity);
            }
            
            return updatedEntity;
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

        public T Get<T>(long id) where T: BaseEntity
        {
            var targetDbSet = GetDbSet<T>();
            return targetDbSet == null ? null : targetDbSet.Find(id);
        }

        public void Delete<T>(T baseObject) where T: BaseEntity
        {
            var targetDbSet = GetDbSet<T>();
            targetDbSet.Remove(baseObject);
        }

        private DbSet<T> GetDbSet<T>() where T : BaseEntity
        {
            DbSet<T> targetDbSet = null;

            PropertyInfo[] propertyInfos = this.db.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.PropertyType != typeof (DbSet<T>)) continue;
                targetDbSet = (DbSet<T>) propertyInfo.GetValue(this.db);
                break;
            }
            return targetDbSet;
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }

        public DbEntityEntry Entry(object entity)
        {
            return db.Entry(entity);
        }
    }
}
