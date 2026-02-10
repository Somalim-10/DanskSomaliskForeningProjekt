using SomaliskDanskForening_Lib.Interfaces;
using SomaliskDanskForening_Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SomaliskDanskForening_Lib.Repo
{
    public class ContactRepositoryDB : IContactRepo
    {
        public int nextId = 1;
        private readonly List<Contact> _contacts = new List<Contact>();

        public ContactRepositoryDB()
        {
            Add(new Contact { Address = "Hovedgaden 1, København", Phone = "+45 12 34 56 78", Email = "kontakt@forening.dk", GoogleMapsUrl = null });
        }

        public Contact? Add(Contact contact)
        {
            if (_contacts.Count >= 1)
            {
                throw new InvalidOperationException("Only one contact entry allowed. Use Update to modify the existing contact.");
            }

            contact.Id = nextId++;
            _contacts.Add(contact);
            return contact;
        }

        public Contact? GetById(int id)
        {
            return _contacts.FirstOrDefault(c => c.Id == id);
        }

        public List<Contact> GetAll()
        {
            return _contacts.ToList();
        }

        public Contact? Update(Contact contact)
        {
            var existing = GetById(contact.Id);
            if (existing == null) return null;
            existing.Address = contact.Address;
            existing.Phone = contact.Phone;
            existing.Email = contact.Email;
            existing.GoogleMapsUrl = contact.GoogleMapsUrl;
            return existing;
        }

        public Contact? Delete(int id)
        {
            var contact = GetById(id);
            if (contact == null) return null;
            _contacts.Remove(contact);
            return contact;
        }
    }
}
