using System;

namespace SomaliskDanskForening_Lib.Models
{
    public class Contact
    {
        private string _address;
        private string _phone;
        private string _email;
        private string? _googleMapsUrl;

        public int Id { get; set; }

        public string Address
        {
            get => _address;
            set
            {
                if (value == null) throw new ArgumentNullException("Address cannot be null");
                if (value.Length > 500) throw new ArgumentException("Address must be less than or equal to 500 characters");
                _address = value;
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                if (value == null) throw new ArgumentNullException("Phone cannot be null");
                if (value.Length > 50) throw new ArgumentException("Phone must be less than or equal to 50 characters");
                _phone = value;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (value == null) throw new ArgumentNullException("Email cannot be null");
                if (value.Length > 200) throw new ArgumentException("Email must be less than or equal to 200 characters");
                _email = value;
            }
        }

        public string? GoogleMapsUrl
        {
            get => _googleMapsUrl;
            set => _googleMapsUrl = value;
        }

        public Contact(int id, string address, string phone, string email, string? googleMapsUrl)
        {
            Id = id;
            Address = address;
            Phone = phone;
            Email = email;
            GoogleMapsUrl = googleMapsUrl;
        }

        public Contact() : this(0, "", "", "", null) { }

        public override string ToString()
        {
            return $"{Address} | {Phone} | {Email} | {GoogleMapsUrl}";
        }
    }
}
