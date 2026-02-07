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
            _evt = new Event(7, "Test Title", _date, 90, "Some description", 14);
        }

        [TestMethod]
        public void Constructor_SetsProperties()
        {
            // Use event created in TestInitialize
            Assert.AreEqual(7, _evt.Id);
            Assert.AreEqual("Test Title", _evt.Title);
            Assert.AreEqual(_date.Date, _evt.Date);
            Assert.AreEqual(90, _evt.Duration);
            Assert.AreEqual("Some description", _evt.Description);
            Assert.AreEqual(14, _evt.StartTime);
        }

        [TestMethod]
        public void Title_Null_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _ = new Event(1, null!, DateTime.Today, 60, "desc", 10);
            });
        }

        [TestMethod]
        public void Title_TooLong_ThrowsArgumentException()
        {
            var longTitle = new string('A', 201);
            Assert.ThrowsException<ArgumentException>(() =>
            {
                _ = new Event(1, longTitle, DateTime.Today, 60, "desc", 10);
            });
        }

        [TestMethod]
        public void StartTime_OutOfRange_ThrowsArgumentOutOfRangeException()
        {
            // Too low
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                var e = new Event();
                e.StartTime = 1;
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
        [DataRow(60)]
        [DataRow(1440)]
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
                _ = new Event(1, "Title", DateTime.Today, 60, null!, 9);
            });
        }

        [TestMethod]
        public void ToString_IncludesKeyValues()
        {
            var evt = new Event(2, "Party", new DateTime(2026, 5, 5), 120, "Fun", 18);
            var str = evt.ToString();

            Assert.IsTrue(str.Contains("Party"));
            Assert.IsTrue(str.Contains("120"));
            Assert.IsTrue(str.Contains("2026-05-05") || str.Contains("05/05/2026") || str.Contains("5/5/2026"));
        }
    }
}
