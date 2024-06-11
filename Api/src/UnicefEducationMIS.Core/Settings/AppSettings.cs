namespace UnicefEducationMIS.Core.Settings
{
    public class AppSettings
    {
        public string Token { get; set; }
        public bool UpdateCampCoordinates { get; set; }
        public string HostUrl { get; set; }
        public string PasswordResetUrl { get; set; }
        public int PasswordResetTokenLifespan { get; set; }
    }
}
