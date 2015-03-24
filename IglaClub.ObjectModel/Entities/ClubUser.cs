namespace IglaClub.ObjectModel.Entities
{
    public class ClubUser : BaseEntity
    {
        public User Users { get; set; }

        public Club Clubs { get; set; }

        public bool IsAdministrator { get; set; }
    }
}
