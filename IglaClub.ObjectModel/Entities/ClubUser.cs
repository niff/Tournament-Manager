using System;
using IglaClub.ObjectModel.Enums;

namespace IglaClub.ObjectModel.Entities
{
    public class ClubUser : BaseEntity
    {
        public User User { get; set; }

        public long UserId { get; set; }

        public Club Club { get; set; }

        public long ClubId { get; set; }

        public bool IsAdministrator { get; set; }

        public DateTime MemberSince { get; set; }

        public MembershipStatus MembershipStatus { get; set; }
    }
}
