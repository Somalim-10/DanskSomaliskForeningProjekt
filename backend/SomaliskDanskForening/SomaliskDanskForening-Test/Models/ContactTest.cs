using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SomaliskDanskForening_Lib.Models;

namespace SomaliskDanskForening_Test
{
    [TestClass]
    public class ContactTest
    {
        private Contact _contact;

        [TestInitialize]
        public void TestInitialize()
        {               
            _contact = new Contact(7, "Address 1", "123", "a@b.com", "http://maps");
        }

        [TestMethod]
        public void Constructor_SetsProperties()
        {
            var c = new Contact(1, "Address 1", "123", "a@b.com", "http://maps");
            Assert.AreEqual(1, c.Id);
            Assert.AreEqual("Address 1", c.Address);
            Assert.AreEqual("123", c.Phone);
            Assert.AreEqual("a@b.com", c.Email);
            Assert.AreEqual("http://maps", c.GoogleMapsUrl);
        }

        [TestMethod]
        public void Address_Null_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _ = new Contact(1, null!, "123", "a@b.com", null);
            });
        }

        [TestMethod]
        public void Address_TooLong_ThrowsArgumentException()
        {
            var longAddr = new string('A', 501);
            Assert.ThrowsException<ArgumentException>(() =>
            {
                _ = new Contact(1, longAddr, "123", "a@b.com", null);
            });
        }

        [TestMethod]
        public void Phone_Null_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _ = new Contact(1, "Addr", null!, "a@b.com", null);
            });
        }

        [TestMethod]
        public void Phone_TooLong_ThrowsArgumentException()
        {
            var longPhone = new string('9', 51);
            Assert.ThrowsException<ArgumentException>(() =>
            {
                _ = new Contact(1, "Addr", longPhone, "a@b.com", null);
            });
        }

        [TestMethod]
        public void Email_Null_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _ = new Contact(1, "Addr", "123", null!, null);
            });
        }

        [TestMethod]
        public void Email_TooLong_ThrowsArgumentException()
        {
            var longEmail = new string('a', 201) + "@x.com";
            Assert.ThrowsException<ArgumentException>(() =>
            {
                _ = new Contact(1, "Addr", "123", longEmail, null);
            });
        }

        [TestMethod]
        public void GoogleMapsUrl_CanBeNull()
        {
            var c = new Contact(2, "Addr", "123", "e@f.com", null);
            Assert.IsNull(c.GoogleMapsUrl);
        }

        [TestMethod]
        public void ToStringTest()
        {
            Assert.AreEqual("Address 1 | 123 | a@b.com | http://maps", _contact.ToString());
        }
    }
}
