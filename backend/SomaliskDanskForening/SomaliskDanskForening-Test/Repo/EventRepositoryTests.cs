using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SomaliskDanskForening_Lib.Repo;
using SomaliskDanskForening_Lib.Models;

namespace SomaliskDanskForening_Test.Repo
{
    [TestClass]
    public class EventRepositoryTests
    {
        private EventRepositoryDB _repo;

        [TestInitialize]
        public void TestInitialize()
        {
            _repo = new EventRepositoryDB();
        }

        [TestMethod]
        public void Add_AssignsIdAndCanBeRetrieved()
        {

            var Added = _repo.Add(new Event { Title = "Test Event", Date = DateTime.Today, StartTime = 12, Duration = 2, Description = "Test Description" });

            Assert.IsNotNull(Added);

            var allEvents = _repo.GetAll();
            Assert.AreEqual(2, allEvents.Count); // 1 from constructor + 1 added

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _repo.Add(new Event { Title = null, Date = DateTime.Today, StartTime = 12, Duration = 2, Description = "Test Description" });
            });
            Assert.ThrowsException<ArgumentException>(() =>
            {
                _repo.Add(new Event { Title = new string('A', 201), Date = DateTime.Today, StartTime = 12, Duration = 2, Description = "Test Description" });
            });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
           {
               _repo.Add(new Event { Title = "Valid Title", Date = DateTime.Today, StartTime = -1, Duration = 2, Description = "Test Description" });
           });

        }

        [TestMethod]
        public void GetAll_ReturnsAllEvents()
        {
            var events = _repo.GetAll();
            Assert.AreEqual(1, events.Count); // Only the default event from constructor
        }

        [TestMethod]
        public void GetById_ReturnsCorrectEvent()
        {
            var evt = _repo.Add(new Event { Title = "Test Event 2", Date = DateTime.Today, StartTime = 14, Duration = 3, Description = "Another Test Description" });
            Event? @event = _repo.GetById(evt.Id);
            Assert.IsNotNull(@event);
            Assert.AreEqual("Test Event 2", @event.Title);
            Assert.AreEqual(14, @event.StartTime);
            Assert.AreEqual(3, @event.Duration);
            Assert.AreEqual("Another Test Description", @event.Description);
            Assert.AreEqual(evt.Id, @event.Id);
            Assert.IsNull(_repo.GetById(999)); // Non-existent ID should return null    


        }
        [TestMethod]
        public void Update_ModifiesExistingEvent()
        {
            var evt = _repo.GetById(1);
            Assert.IsNotNull(evt);
            evt.Title = "Updated Title";
            var updated = _repo.Update(evt);
            Assert.IsNotNull(updated);
            Assert.AreEqual("Updated Title", updated.Title);
        }
        [TestMethod]
        public void Delete_RemovesEvent()
        {
            var deleted = _repo.Delete(1);
            Assert.IsNotNull(deleted);
            Assert.AreEqual(1, deleted.Id);
            var evt = _repo.GetById(1);
            Assert.IsNull(evt);

        }
    }

}
