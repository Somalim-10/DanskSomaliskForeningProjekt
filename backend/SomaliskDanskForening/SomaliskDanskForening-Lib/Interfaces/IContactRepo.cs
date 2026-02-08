using System.Collections.Generic;
using SomaliskDanskForening_Lib.Models;

namespace SomaliskDanskForening_Lib.Interfaces
{
    public interface IContactRepo
    {
        Contact? Add(Contact contact);
        Contact? GetById(int id);
        List<Contact> GetAll();
        Contact? Update(Contact contact);
        Contact? Delete(int id);
    }
}
