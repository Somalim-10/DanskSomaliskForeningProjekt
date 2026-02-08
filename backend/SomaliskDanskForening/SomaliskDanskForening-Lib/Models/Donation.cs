using System;

namespace SomaliskDanskForening_Lib.Models
{
    public class Donation
    {
        private string _text;
        private string? _mobilePay;

        public int Id { get; set; }

        public string Text
        {
            get => _text;
            set
            {
                if (value == null) throw new ArgumentNullException("Text cannot be null");
                if (value.Length > 2000) throw new ArgumentException("Text must be less than or equal to 2000 characters");
                _text = value;
            }
        }

        public string? MobilePay
        {
            get => _mobilePay;
            set => _mobilePay = value;
        }

        public Donation(int id, string text, string? mobilePay)
        {
            Id = id;
            Text = text;
            MobilePay = mobilePay;
        }

        public Donation() : this(0, "Støt os venligst", null) { }

        public override string ToString()
        {
            return $"{Text} | MobilePay: {MobilePay}";
        }
    }
}
