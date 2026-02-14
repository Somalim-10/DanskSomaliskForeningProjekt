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
    public class EventRepositoryDB : IEventRepo
    {
        //public int nextId = 1;
        //private readonly List<Event> _events = new List<Event>();

        public readonly ForeningDbContext _events;

        public EventRepositoryDB( ForeningDbContext foreningDbContext)
        {
            _events = foreningDbContext;

        }
        public Event? Add(Event evt)
        {
            _events.Events.Add(evt);
            _events.SaveChanges();
            return evt;
        }
       
          

        

        public Event? GetById(int id)
        {
         
            return _events.Events.FirstOrDefault(e => e.Id == id);
        }
        public List<Event> GetAll()
        {
            return _events.Events.ToList();



        }
        public Event? Update(int id, Event evt)
        {
            var existing = GetById(evt.Id);
            if (existing == null) return null;
            evt.Id = id;
            existing.Title = evt.Title;
            existing.Date = evt.Date;
            existing.StartTime = evt.StartTime;
            existing.Duration = evt.Duration;
            existing.Description = evt.Description;
            _events.SaveChanges();
            return existing;
        }

        public Event? Delete(int id)
        {
          
            var existing = GetById(id);
            if (existing == null) return null;
            _events.Events.Remove(existing);
            _events.SaveChanges();
            return existing;
        }
    }
}
