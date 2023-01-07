using AutoMapper;
using ContactsApp.ViewModel;
using ContactsApp;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApp.Test
{
    public class TestHelperClass
    {
        public static List<Contact> GetTestContacts()
        {
            var contacts = new List<Contact>();
            contacts.Add(new Contact()
            {
                Id = 1,
                FirstName = "Lionel",
                LastName = "Messi",
                PhoneNumber = "0904787",
                Email = "lionelmessi@gmail.com"
            });
            contacts.Add(new Contact()
            {
                Id = 2,
                FirstName = "Andres",
                LastName = "Iniesta",
                PhoneNumber = "090348976",
                Email = "andresiniesta@gmail.com"
            });
            return contacts;
        }

        public static List<ContactViewModel> GetTestContactsViewModel()
        {
            var contacts = new List<ContactViewModel>();
            contacts.Add(new ContactViewModel()
            {
                Id = 1,
                FirstName = "Lionel",
                LastName = "Messi",
                PhoneNumber = "0904787",
                Email = "lionelmessi@gmail.com"
            });
            contacts.Add(new ContactViewModel()
            {
                Id = 2,
                FirstName = "Andres",
                LastName = "Iniesta",
                PhoneNumber = "090348976",
                Email = "andresiniesta@gmail.com"
            });
            return contacts;
        }
        public static IMapper GetMapper()
        {
            var mappingProfile = new ContactProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            return new Mapper(configuration);
        }
    }
}
