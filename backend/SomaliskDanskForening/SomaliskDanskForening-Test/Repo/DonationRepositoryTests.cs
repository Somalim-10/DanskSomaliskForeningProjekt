using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SomaliskDanskForening_Lib.Repo;
using SomaliskDanskForening_Lib.Models;

namespace SomaliskDanskForening_Test.Repo
{
    [TestClass]
    public class DonationRepositoryTests
    {
        private DonationRepositoryDB _repo;

        [TestInitialize]
        public void TestInitialize()
        {
            _repo = new DonationRepositoryDB();
        }

        [TestMethod]
        public void Add_AssignsIdAndCanBeRetrieved()
        {
            // When a donation entry already exists, adding another must be rejected
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                _repo.Add(new Donation { Text = "Test Donation", MobilePay = null });
            });

            // Remove default and then add should succeed
            _repo.Delete(1);
            var added = _repo.Add(new Donation { Text = "Test Donation", MobilePay = null });
            Assert.IsNotNull(added);

            var all = _repo.GetAll();
            Assert.AreEqual(1, all.Count); // only the newly added donation

            // Validation checks (model throws on invalid values)
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _repo.Add(new Donation { Text = null, MobilePay = null });
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                _repo.Add(new Donation { Text = new string('A', 2001), MobilePay = null });
            });
        }

        [TestMethod]
        public void GetAll_ReturnsAllDonations()
        {
            var donations = _repo.GetAll();
            Assert.AreEqual(1, donations.Count);
        }

        [TestMethod]
        public void GetById_ReturnsCorrectDonation()
        {
            var d = _repo.GetById(1);
            Assert.IsNotNull(d);
            Assert.AreEqual(1, d.Id);
            Assert.AreEqual("Støt foreningen — dit bidrag gør en forskel", d.Text);
            Assert.IsNull(d.MobilePay);
            Assert.IsNull(_repo.GetById(999));


        }

        [TestMethod]
        public void Update_ModifiesExistingDonation()
        {
            var d = _repo.GetById(1);
            Assert.IsNotNull(d);
            d.Text = "Updated Donation";
            var updated = _repo.Update(d);
            Assert.IsNotNull(updated);
            Assert.AreEqual("Updated Donation", updated.Text);
        }

        [TestMethod]
        public void Delete_RemovesDonation()
        {
            var deleted = _repo.Delete(1);
            Assert.IsNotNull(deleted);
            Assert.AreEqual(1, deleted.Id);
            var d = _repo.GetById(1);
            Assert.IsNull(d);
        }
    }
}
