using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SomaliskDanskForening_Lib.Repo;
using SomaliskDanskForening_Lib.Models;
using Microsoft.EntityFrameworkCore;

namespace SomaliskDanskForening_Test.Repo
{
    [TestClass]
    public class ContactRepositoryTests
    {
        private ContactRepositoryDB _repo;

        [TestInitialize]
        public void TestInitialize()
        {

            // Use a fresh in-memory database for each test
            var options = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<SomaliskDanskForening_Lib.Data.ForeningDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new SomaliskDanskForening_Lib.Data.ForeningDbContext(options);
            _repo = new ContactRepositoryDB(context);
            // Seed with a default contact
            _repo.Add(new Contact
            {
                Address = "Hovedgaden 1, København",
                Phone = "+45 12 34 56 78",
                Email = "kontakt@forening.dk",
                GoogleMapsUrl = null
            });

        }

        [TestMethod]
        public void Add_AssignsIdAndCanBeRetrieved()
        {
            // When a contact already exists, Add must not allow another
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                _repo.Add(new Contact { Address = "Test Address", Phone = "+45 11 22 33 44", Email = "test@example.com", GoogleMapsUrl = null });
            });

            // Remove default and then Add should succeed
            _repo.Delete(1);
            var added = _repo.Add(new Contact { Address = "Test Address", Phone = "+45 11 22 33 44", Email = "test@example.com", GoogleMapsUrl = null });
            Assert.IsNotNull(added);

            var all = _repo.GetAll();
            Assert.AreEqual(1, all.Count); // only the newly added contact

            // Validation checks (model throws on invalid values)
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _repo.Add(new Contact { Address = null, Phone = "+45 11 22 33 44", Email = "test@example.com", GoogleMapsUrl = null });
            });

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _repo.Add(new Contact { Address = "Valid", Phone = null, Email = "test@example.com", GoogleMapsUrl = null });
            });

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _repo.Add(new Contact { Address = "Valid", Phone = "+45 11 22 33 44", Email = null, GoogleMapsUrl = null });
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                _repo.Add(new Contact { Address = new string('A', 501), Phone = "+45 11 22 33 44", Email = "test@example.com", GoogleMapsUrl = null });
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                _repo.Add(new Contact { Address = "Valid", Phone = new string('1', 51), Email = "test@example.com", GoogleMapsUrl = null });
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                _repo.Add(new Contact { Address = "Valid", Phone = "+45 11 22 33 44", Email = new string('a', 201), GoogleMapsUrl = null });
            });
        }

        [TestMethod]
        public void GetAll_ReturnsAllContacts()
        {
            var contacts = _repo.GetAll();
            Assert.AreEqual(1, contacts.Count);
        }

        [TestMethod]
        public void GetById_ReturnsCorrectContact()
        {
            // Use the seeded single contact (Id = 1)
            var c = _repo.GetById(1);
            Assert.IsNotNull(c);
            Assert.IsTrue(c.Address.Contains("Hovedgaden 1"));
            Assert.AreEqual("+45 12 34 56 78", c.Phone);
            Assert.AreEqual("kontakt@forening.dk", c.Email);
            Assert.AreEqual(1, c.Id);
            Assert.IsNull(_repo.GetById(999));
        }

        [TestMethod]
        public void Update_ModifiesExistingContact()
        {
            var c = _repo.GetById(1);
            Assert.IsNotNull(c);
            c.Address = "Updated Address";
            var updated = _repo.Update(c);
            Assert.IsNotNull(updated);
            Assert.AreEqual("Updated Address", updated.Address);
        }

        [TestMethod]
        public void Delete_RemovesContact()
        {
            var deleted = _repo.Delete(1);
            Assert.IsNotNull(deleted);
            Assert.AreEqual(1, deleted.Id);
            var c = _repo.GetById(1);
            Assert.IsNull(c);
        }
    }
}
