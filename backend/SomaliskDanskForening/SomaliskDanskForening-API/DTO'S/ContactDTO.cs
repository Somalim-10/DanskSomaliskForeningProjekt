using SomaliskDanskForening_Lib.Models;

namespace SomaliskDanskForening_API.DTO_S
{
    public class ContactDTO
    {
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? GoogleMapsUrl { get; set; }

        public Contact ToContact() => new Contact(0, Address, Phone, Email, GoogleMapsUrl);
    }
}
