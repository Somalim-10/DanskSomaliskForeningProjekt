using SomaliskDanskForening_Lib.Interfaces;
using SomaliskDanskForening_Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SomaliskDanskForening_Lib.Repo
{
    public class DonationRepositoryDB : IDonationRepo
    {
        public int nextId = 1;
        private readonly List<Donation> _donations = new List<Donation>();

        public DonationRepositoryDB()
        {
            Add(new Donation { Text = "Støt foreningen — dit bidrag gør en forskel", MobilePay = null });
        }

        public Donation? Add(Donation donation)
        {
            if (_donations.Count >= 1)
            {
                throw new InvalidOperationException("Only one donation entry allowed. Use Update to modify the existing donation.");
            }

            donation.Id = nextId++;
            _donations.Add(donation);
            return donation;
        }

        public Donation? GetById(int id)
        {
            return _donations.FirstOrDefault(d => d.Id == id);
        }

        public List<Donation> GetAll()
        {
            return _donations.ToList();
        }

        public Donation? Update(Donation donation)
        {
            var existing = GetById(donation.Id);
            if (existing == null) return null;
            existing.Text = donation.Text;
            existing.MobilePay = donation.MobilePay;
            return existing;
        }

        public Donation? Delete(int id)
        {
            var donation = GetById(id);
            if (donation == null) return null;
            _donations.Remove(donation);
            return donation;
        }
    }
}
