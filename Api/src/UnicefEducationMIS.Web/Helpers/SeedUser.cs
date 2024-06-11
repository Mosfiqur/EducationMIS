namespace UnicefEducationMIS.Web.Helpers
{
    public class SeedUser
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }        
        public int DesignationId { get; set; }
        public string PhoneNumber { get; set; }
    }
}
