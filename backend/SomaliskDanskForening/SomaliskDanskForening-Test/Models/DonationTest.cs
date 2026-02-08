using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SomaliskDanskForening_Lib.Models;

namespace SomaliskDanskForening_Test
{
    [TestClass]
    public class DonationTest
    {
        private Donation _donation;

        [TestInitialize]
        public void TestInitialize()
        {
            _donation = new Donation(7, "Support us", "mp123");
        }

        [TestMethod]
        public void Constructor_SetsProperties()
        {
            var d = new Donation(1, "Text", "mp");
            Assert.AreEqual(1, d.Id);
            Assert.AreEqual("Text", d.Text);
            Assert.AreEqual("mp", d.MobilePay);
        }

        [TestMethod]
        public void Text_Null_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _ = new Donation(1, null!, null);
            });
        }

        [TestMethod]
        public void Text_TooLong_ThrowsArgumentException()
        {
            var longText = new string('A', 2001);
            Assert.ThrowsException<ArgumentException>(() =>
            {
                _ = new Donation(1, longText, null);
            });
        }

        [TestMethod]
        public void MobilePay_CanBeNull()
        {
            var d = new Donation(2, "T", null);
            Assert.IsNull(d.MobilePay);
        }

        [TestMethod]
        public void ToStringTest()
        {
            Assert.AreEqual("Support us | MobilePay: mp123", _donation.ToString());
        }
    }
}
