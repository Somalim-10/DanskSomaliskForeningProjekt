using System;
using SomaliskDanskForening_Lib.Models;

namespace SomaliskDanskForening_API.DTO_S
{
    public class EventsDTO
    {
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int startTime { get; set; }
        public int duration { get; set; }
        public string Description { get; set; } = string.Empty;

        // Korrekt opkald til Event-konstruktøren (id, title, date, durationHours, description, startTime)
        public Event ToEventDTO() => new Event(0, Title, Date, duration, Description, startTime);
    }
}
