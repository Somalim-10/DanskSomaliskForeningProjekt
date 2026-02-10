using System.Collections.Generic;
using SomaliskDanskForening_Lib.Models;

namespace SomaliskDanskForening_Lib.Interfaces
{
    public interface IEventRepo
    {
        Event? Add(Event evt);
        Event? GetById(int id);
       
        List<Event> GetAll();
        Event? Update(int id,Event evt);
        Event? Delete(int id);
    }
}
