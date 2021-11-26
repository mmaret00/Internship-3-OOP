using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Internship_3_OOP.Entities;

namespace Internship_3_OOP
{
    public enum Preference
    {
        favorite,
        normal,
        blocked
    }

    public class Contact
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public Preference Pref;

        public Contact()
        {
            Name = "";
            PhoneNumber = "";
            Pref = Preference.normal;
        }

        public Contact(string _name, string _phoneNumber, Preference _pref)
        {
            Name = _name;
            PhoneNumber = _phoneNumber;
            Pref = _pref;
        }
    }
}
