namespace IglaClub.Web.Helpers
{
    public static class ValidationHelper
    {
        public static bool IsValidEmailAddress(string emailAddress)
        {
            return new System.ComponentModel.DataAnnotations
                                .EmailAddressAttribute()
                                .IsValid(emailAddress);
        }
    }
}