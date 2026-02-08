using System.Collections.Generic;
using SomaliskDanskForening_Lib.Models;

namespace SomaliskDanskForening_Lib.Interfaces
{
    public interface IDonationRepo
    {
        Donation? Add(Donation donation);
        Donation? GetById(int id);
        List<Donation> GetAll();
        Donation? Update(Donation donation);
        Donation? Delete(int id);
    }
}
