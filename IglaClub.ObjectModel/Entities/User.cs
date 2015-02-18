using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IglaClub.ObjectModel.Entities
{
    public class User : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Lastname { get; set; }

        [MaxLength(100)]
        public string Nickname { get; set; }

        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Login is required")]
        [MaxLength(100)]
        public string Login { get; set; }

        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        public virtual IList<Club> Clubs { get; set; }
        
        public string GetDisplayName()
        {
            if(!string.IsNullOrEmpty(this.Nickname))
                return this.Nickname;
            return this.Login;
        }

        public string DisplayName
        {
            get { return GetDisplayName(); }
        }

    }
}