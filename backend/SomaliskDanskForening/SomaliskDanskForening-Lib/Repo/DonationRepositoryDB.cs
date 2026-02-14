using SomaliskDanskForening_Lib.Data;
using SomaliskDanskForening_Lib.Interfaces;
using SomaliskDanskForening_Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomaliskDanskForening_Lib.Repo
{
    public class DonationRepositoryDB : IDonationRepo
    {
        //public int nextId = 1;
        //private readonly List<Donation> _donations = new List<Donation>();
        public readonly ForeningDbContext _donations;

        public DonationRepositoryDB(ForeningDbContext donations)
        {
            _donations = donations;
        }

        public Donation? Add(Donation donation)
        {
         
            if (_donations.Donations.Any()) throw new InvalidOperationException("Only one donation entry allowed");
            _donations.Donations.Add(donation);
            _donations.SaveChanges();
            return donation;



        }

        public Donation? GetById(int id)
        {
           
            return _donations.Donations.FirstOrDefault(d => d.Id == id);
        }

        public List<Donation> GetAll()
        {
            return _donations.Donations.ToList();
        }

        public Donation? Update(Donation donation)
        {
            var existing = GetById(donation.Id);
            if (existing == null) return null;
            existing.Text = donation.Text;
            existing.MobilePay = donation.MobilePay;
            _donations.SaveChanges();
            return existing;
        }

        public Donation? Delete(int id)
        {
            var donation = GetById(id);
            if (donation == null) return null;
            _donations.Remove(donation);
                _donations.SaveChanges();
            return donation;
        }
    }
}
