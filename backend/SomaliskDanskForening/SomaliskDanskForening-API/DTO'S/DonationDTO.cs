using SomaliskDanskForening_Lib.Models;

namespace SomaliskDanskForening_API.DTO_S
{
    public class DonationDTO
    {
        public string Text { get; set; } = string.Empty;
        public string? MobilePay { get; set; }

        public Donation ToDonation() => new Donation(0, Text, MobilePay);
    }
}
