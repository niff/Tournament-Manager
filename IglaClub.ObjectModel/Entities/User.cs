﻿using System;
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
        [MaxLength(250)]
        [StringLength(250)]
        public string Login { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        public virtual IList<ClubUser> ClubUsers { get; set; }
        
        public string GetDisplayName()
        {
            if(!string.IsNullOrEmpty(this.Nickname))
                return this.Nickname;
            
            if (!string.IsNullOrEmpty(this.Name) && !string.IsNullOrEmpty(this.Lastname))
                return string.IsNullOrEmpty(this.Name) ? this.Lastname : this.Name + " " + this.Lastname;
            
            return this.Login;
        }

        public string DisplayName
        {
            get { return GetDisplayName(); }
        }

        public DateTime CreationDate { get; set; }

        public DateTime? LastLoginTs { get; set; }

        public bool Valid { get; set; }
    }
}