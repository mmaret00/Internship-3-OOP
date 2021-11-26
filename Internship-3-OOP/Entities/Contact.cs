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

        static public void AddNewContact(Dictionary<Contact, Call[]> directory)
        {
            Console.WriteLine("Unesi ime novog kontakta:");
            var name = Console.ReadLine();

            Console.WriteLine("Unesi broj:");
            var number = CheckIfNumberIsTaken(directory);

            Preference prefer = GetValidPreference();

            var _contact = new Contact(name, number, prefer);
            directory.Add(_contact, null);
            Console.Clear();
            Console.WriteLine("Unešen je kontakt " + name + " s brojem " + number + " i preferencom " + prefer + ".");
            Program.ReturnToMenu();
        }

        static public Preference GetValidPreference()
        {
            int exit = 0;
            while (0 == exit)
            {
                Console.WriteLine("Unesi preferenciju (1 - favorite, 2 - normal, 3 - blocked):");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                    case "favorite":
                        return Preference.favorite;
                    case "2":
                    case "normal":
                        return Preference.normal;
                    case "3":
                    case "blocked":
                        return Preference.blocked;
                    default:
                        Console.WriteLine("Nedopušten unos, unesite broj (1-3) ili jednu od tri ponuđene riječi. Molimo unesite ispočetka:");
                        break;
                }
            }

            return 0;
        }

        static public string CheckIfNumberIsTaken(Dictionary<Contact, Call[]> directory)
        {
            var number = "";
            var exit = 0;
            while (0 == exit)
            {
                var exists = 0;
                number = Console.ReadLine();

                foreach (var item in directory)
                {
                    if (number == item.Key.PhoneNumber)
                    {
                        exists++;
                        Console.WriteLine("Taj broj već postoji u imeniku, molimo unesite ga ispočetka:");
                    }
                }

                if (!NumberDigitsCheck(number))
                {
                    Console.WriteLine("Broj treba sadržavati samo brojeve! Unesite ga opet:");
                    exists++;
                }

                if (0 == exists) exit = 1;
            }

            return number;
        }

        static public bool NumberDigitsCheck(string number)
        {
            foreach (var item in number)
                if (!char.IsDigit(item))
                    return false;

            return true;
        }

        static public string GetValidPhoneNumber()
        {
            int repeat;
            string findNumber;

            do
            {
                repeat = 0;
                findNumber = (Console.ReadLine().Trim());

                if (!NumberDigitsCheck(findNumber))
                {
                    Console.WriteLine("Broj treba sadržavati samo brojeve! Unesite ga opet:");
                    repeat = 1;
                }

            } while (0 != repeat);

            return findNumber;
        }

        static public void DeleteContact(Dictionary<Contact, Call[]> directory)
        {
            Console.WriteLine("Unesi broj kontakta kojeg želiš obrisati: ");
            var deleteNumber = GetValidPhoneNumber();
            Contact contactToDelete = null;

            var exists = 0;

            foreach (var item in directory)
                if (deleteNumber == item.Key.PhoneNumber)
                {
                    exists++;
                    contactToDelete = item.Key;
                }

            if (0 == exists)
                Console.WriteLine("Imenik ne sadrži taj kontakt.");

            else if (1 == exists)
            {
                var correct = 0;
                while (0 == correct)
                {

                    correct = 1;
                    var deleteName = contactToDelete.Name;
                    directory.Remove(contactToDelete);
                    Console.WriteLine("Stanovnik " + deleteName + " s brojem " + deleteNumber + " je obrisan.");
                }
            }
            Program.ReturnToMenu();
        }

        static public void EditPreference(Dictionary<Contact, Call[]> directory)
        {
            Console.WriteLine("Unesi broj kontakta kojem želiš promijeniti preferencu:");
            string changeNumber = GetValidPhoneNumber();
            int correct = 0, exists = 0;

            foreach (var item in directory)
                if (changeNumber == item.Key.PhoneNumber)
                {
                    exists++;
                    while (0 == correct)
                    {
                        correct = 1;
                        Console.WriteLine("Unesi novu preferencu:");
                        Preference newPreference = Contact.GetValidPreference();
                        item.Key.Pref = newPreference;
                        Console.WriteLine("Preferenca je promijenjena u " + newPreference + ".");
                    }
                }

            if (0 == exists)
            {
                Console.WriteLine("Ne postoji kontakt s brojem " + changeNumber + ".");
            }
            Program.ReturnToMenu();
        }

        static public void ManageContact(Dictionary<Contact, Call[]> directory)
        {
            Console.WriteLine("Unesite broj kontakta:");
            var number = GetValidPhoneNumber();

            Contact contactToManage = null;
            var exists = 0;
            foreach (var item in directory)
                if (number == item.Key.PhoneNumber)
                {
                    contactToManage = item.Key;
                    var nameToManage = item.Key.Name;
                    exists++;
                }

            if (0 == exists) Console.WriteLine("Broj ne postoji u imeniku.");
            else if (1 == exists)
            {
                Program.Submenu(directory, contactToManage);
            }
        }

        static public void PrintContacts(Dictionary<Contact, Call[]> directory, int choice)
        {
            if (1 != choice) Console.WriteLine("Kontakti:");

            foreach (var item in directory)
            {
                Console.WriteLine(item.Key.Name + ", " + item.Key.PhoneNumber + " (" + item.Key.Pref + ")");
                if (1 == choice && item.Value != null)
                {
                    Console.WriteLine("\nDatum i vrijeme poziva:\t\tStatus:");
                    for (int i = 0; i < item.Value.Length; i++)
                        Console.WriteLine(item.Value[i].CallSetupTime + "\t\t" + item.Value[i]._callStatus);
                    Console.WriteLine("\n===================\n");
                }
                else if (1 == choice && item.Value == null)
                {
                    Console.WriteLine("\nNema poziva s kontaktom.");
                    Console.WriteLine("\n===================\n");
                }
            }
            if (0 == choice || 1 == choice)
            {
                Program.ReturnToMenu();
            }
        }

        static public void PrintCallsOfAContact(Dictionary<Contact, Call[]> directory, string number)
        {
            foreach (var item in directory)
            {
                if (number == item.Key.PhoneNumber)
                {
                    Console.WriteLine(item.Key.Name + ", " + item.Key.PhoneNumber + " (" + item.Key.Pref + ")");
                    if (item.Value != null)
                    {
                        var sortedCalls = Call.CreateSortedListOfCalls(item.Value);

                        Console.WriteLine("\nDatum i vrijeme poziva:\t\tStatus:");
                        for (int i = 0; i < item.Value.Length; i++)
                        {
                            Console.WriteLine(sortedCalls[i].Item1 + "\t\t" + sortedCalls[i].Item2);
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nNema poziva s kontaktom.");
                    }
                }
            }
            Program.ReturnToMenu();
        }
    }
}
