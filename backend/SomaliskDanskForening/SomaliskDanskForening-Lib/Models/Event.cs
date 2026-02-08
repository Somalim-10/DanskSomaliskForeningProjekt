using System;

namespace SomaliskDanskForening_Lib.Models
{
    public class Event
    {
        private string _title;
        private DateTime _date;
        private int _startTime;
        private int _duration;
        private string _description;

        public int Id { get; set; }

        public string Title
        {
            get => _title;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Title cannot be null");
                }
                if (value.Length < 1)
                {
                    throw new ArgumentException("Title must be at least 1 character");
                }
                if (value.Length > 200)
                {
                    throw new ArgumentException("Title must be less than or equal to 200 characters");
                }

                _title = value;
            }
        }
        public int StartTime
        {
            get => _startTime;
            set
            {
                // StartTime expressed as hour of day (0-23)
                if (value < 0 || value > 23)
                {
                    throw new ArgumentOutOfRangeException("StartTime must be between 0 and 23: " + value);
                }
                _startTime = value;
            }
        }

        public DateTime Date
        {
            get => _date;
            set => _date = value.Date;
        }


        // Duration expressed in hours
        public int Duration
        {
            get => _duration;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Duration (hours) must be non-negative: " + value);
                }
                _duration = value;

            }

        }

        public string Description
        {
            get => _description;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Description cannot be null");
                }
                if (value.Length > 2000)
                {
                    throw new ArgumentException("Description cannot exceed 2000 characters");
                }
                _description = value;
            }
        }



        public Event(int id, string title, DateTime date, int durationHours, string description, int startTime)
        {
            Id = id;
            Title = title;
            Date = date;
            Duration = durationHours;
            Description = description;
            StartTime = startTime;
        }

        public Event() : this(0, "Default Title", DateTime.Today, 1, "Default Description", 12)
        {
        }

        public override string ToString()
        {
            return $"{Title} ({Date:yyyy-MM-dd} {StartTime:00}:00, {Duration}h)";
        }




    }
}
