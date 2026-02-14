using SomaliskDanskForening_Lib.Data;
using SomaliskDanskForening_Lib.Interfaces;
using SomaliskDanskForening_Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SomaliskDanskForening_Lib.Repo
{
    public class ContactRepositoryDB : IContactRepo
    {
        //public int nextId = 1;
        //private readonly List<Contact> _contacts = new List<Contact>();
        public readonly ForeningDbContext _contacts;
        public ContactRepositoryDB( ForeningDbContext contact)
        {
            _contacts = contact;

        }

        public Contact? Add(Contact contact)
        {
          
           if (_contacts.Contacts.Any()) throw new InvalidOperationException("Only one contact can be added");
            _contacts.Contacts.Add(contact);
            _contacts.SaveChanges();
            return contact;
        }

        public Contact? GetById(int id)
        {
            return _contacts.Contacts.FirstOrDefault(c => c.Id == id);
        }

        public List<Contact> GetAll()
        {
            return _contacts.Contacts.ToList();
        }

        public Contact? Update(Contact contact)
        {
            var existing = GetById(contact.Id);
            if (existing == null) return null;
            existing.Address = contact.Address;
            existing.Phone = contact.Phone;
            existing.Email = contact.Email;
            existing.GoogleMapsUrl = contact.GoogleMapsUrl;
            _contacts.SaveChanges();
            return existing;
        }

        public Contact? Delete(int id)
        {
            var contact = GetById(id);
            if (contact == null) return null;
            _contacts.Remove(contact);
            _contacts.SaveChanges();
            return contact;
        }
    }
}
