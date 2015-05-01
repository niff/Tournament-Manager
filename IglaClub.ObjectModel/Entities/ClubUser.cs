using System;
using IglaClub.ObjectModel.Enums;

namespace IglaClub.ObjectModel.Entities
{
    public class ClubUser : BaseEntity
    {
        public virtual User User { get; set; }

        public long UserId { get; set; }

        public virtual Club Club { get; set; }

        public long ClubId { get; set; }

        public bool IsAdministrator { get; set; }

        public DateTime MemberSince { get; set; }

        public MembershipStatus MembershipStatus { get; set; }

        //todo add migration for remove pk from userid i clubid, add pk to ID, set id to notnull, set values for id existing rows
    }
}
