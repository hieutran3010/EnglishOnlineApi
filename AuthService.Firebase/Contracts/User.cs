namespace AuthService.Firebase.Contracts
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public string PhoneNumber { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string[] Roles { get; set; }
        public string AvatarUrl { get; set; }
        public bool Disabled { get; set; }
    }
}