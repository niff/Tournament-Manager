using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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