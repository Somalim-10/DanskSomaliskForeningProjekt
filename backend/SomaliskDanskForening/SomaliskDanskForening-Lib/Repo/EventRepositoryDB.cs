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
        public int nextId = 1;
        private readonly List<Event> _events = new List<Event>();

        public EventRepositoryDB()
        {
            Add(new Event { Title = "Somalisk Kultur Festival", Date = new DateTime(2024, 7, 15), StartTime = 18, Duration = 4, Description = "En fejring af somalisk kultur med mad, musik og dans." });

        }
        public Event? Add(Event evt)
        {
            evt.Id = nextId++;
            _events.Add(evt);
            return evt;
        }

        public Event? GetById(int id)
        {
            return _events.FirstOrDefault(e => e.Id == id);
        }
        public List<Event> GetAll()
        {
            return _events.ToList();
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
            return existing;
        }

        public Event? Delete(int id)
        {
            var evt = GetById(id);
            if (evt == null) return null;
            _events.Remove(evt);
            return evt;
        }
    }
}
