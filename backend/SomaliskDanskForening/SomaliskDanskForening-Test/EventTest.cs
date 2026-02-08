using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SomaliskDanskForening_Lib.Models;

namespace SomaliskDanskForening_Test
{
    [TestClass]
    public class EventTest
    {
        private DateTime _date;
        private Event _evt;

        [TestInitialize]
        public void TestInitialize()
        {
            _date = new DateTime(2026, 1, 2);
            _evt = new Event(7, "Test Title", _date, 3, "Some description", 14);
        }

        [TestMethod]
        public void Constructor_SetsProperties()
        {

            var evtLocal = new Event(1, "Title", _date, 1, "Description", 10);
            Assert.AreEqual(1, evtLocal.Id);
            Assert.AreEqual("Title", evtLocal.Title);
            Assert.AreEqual(_date.Date, evtLocal.Date);
            Assert.AreEqual(1, evtLocal.Duration);
            Assert.AreEqual("Description", evtLocal.Description);
        }

        [TestMethod]
        public void Title_Null_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _ = new Event(1, null!, DateTime.Today, 1, "desc", 10);
            });
        }

        [TestMethod]
        public void Title_TooLong_ThrowsArgumentException()
        {
            var longTitle = new string('A', 201);
            Assert.ThrowsException<ArgumentException>(() =>
            {
                _ = new Event(1, longTitle, DateTime.Today, 1, "desc", 10);
            });
        }

        [TestMethod]
        public void StartTime_OutOfRange_ThrowsArgumentOutOfRangeException()
        {
            // Too low
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                var e = new Event();
                e.StartTime = -1; // fixed: use -1 for out-of-range low value
            });

            // Too high
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                var e = new Event();
                e.StartTime = 24;
            });
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(-5)]
        [DataRow(int.MinValue)]
        public void Duration_Invalid_ThrowsArgumentOutOfRangeException(int duration)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                _ = new Event(1, "Title", DateTime.Today, duration, "desc", 9);
            });
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(24)]
        [DataRow(168)]
        public void Duration_Valid_SetsValue(int duration)
        {
            var e = new Event();
            e.Duration = duration;
            Assert.AreEqual(duration, e.Duration);
        }

        [TestMethod]
        public void Description_Null_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _ = new Event(1, "Title", DateTime.Today, 1, null!, 9);
            });
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Assert.AreEqual(
                "Test Title (2026-01-02 14:00, 3h)",
                _evt.ToString()
            );
        }


    }
}
