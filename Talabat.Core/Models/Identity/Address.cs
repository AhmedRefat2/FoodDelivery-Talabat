namespace Talabat.Core.Models.Identity
{
    public class Address : BaseModel
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string ApplicationUserId{ get; set; } // Forign Key
        public ApplicationUser User{ get; set; } = null!; // Navigation Property [One]
    }
}