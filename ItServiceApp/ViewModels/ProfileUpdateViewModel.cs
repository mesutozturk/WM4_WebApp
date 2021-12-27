namespace ItServiceApp.ViewModels
{
    public class ProfileUpdateViewModel
    {
        public UserProfileViewModel UserProfileViewModel { get; set; } = new();
        public PasswordUpdateViewModel PasswordUpdateViewModel { get; set; } = new();
    }
}
