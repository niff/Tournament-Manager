using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IglaClub.ObjectModel.Entities
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }

        public BaseEntity()
        {
            //Id = 0;
        }
        //[Timestamp]
        //public Byte[] LastUpdateTs { get; set; }
    }
}