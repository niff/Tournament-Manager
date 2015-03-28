namespace IglaClub.ObjectModel.Entities
{
    public class ClubUser : BaseEntity
    {
        public User User { get; set; }

        public Club Club { get; set; }

        public bool IsAdministrator { get; set; }
    }
}
