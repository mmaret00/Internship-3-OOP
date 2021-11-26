using System;
using System.Collections.Generic;
using Internship_3_OOP.Entities;

namespace Internship_3_OOP
{
    class Program
    {
        public static Random RandomNumberGenerator = new();
        public static int RandomSeconds => RandomNumberGenerator.Next(20);
        public static int RandomCallStatus => RandomNumberGenerator.Next(3);

        static void Main()
        {
            var directory = new Dictionary<Contact, Call[]>() { };
            AddingDefaultContactsAndCalls(directory);
            Menu(directory);
        }

        static void Menu(Dictionary<Contact, Call[]> directory)
        {
            var exit = 0;
            while (1 != exit)
            {
                Console.WriteLine("Odaberite akciju:\n" +
                "1 - Ispis svih kontakata\n" +
                "2 - Dodavanje novog kontakta u imenik\n" +
                "3 - Brisanje kontakta iz imenika\n" +
                "4 - Promjena preference kontakta\n" +
                "5 - Upravljanje kontaktom\n" +
                "6 - Ispis svih poziva\n" +
                "0 - Izlaz iz aplikacije");

                int.TryParse(Console.ReadLine().Trim(), out int choice);

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        Contact.PrintContacts(directory, 0);
                        break;
                    case 2:
                        Console.Clear();
                        Contact.AddNewContact(directory);
                        break;
                    case 3:
                        Console.Clear();
                        Contact.PrintContacts(directory, 2);
                        Contact.DeleteContact(directory);
                        break;
                    case 4:
                        Console.Clear();
                        Contact.PrintContacts(directory, 2);
                        Contact.EditPreference(directory);
                        break;
                    case 5:
                        Console.Clear();
                        Contact.PrintContacts(directory, 2);
                        Contact.ManageContact(directory);
                        break;
                    case 6:
                        Console.Clear();
                        Contact.PrintContacts(directory, 1);
                        break;
                    case 0:
                        Console.Clear();
                        exit = 1;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Molimo unesite jedan od dopuštenih brojeva (0-6)");
                        break;
                }
            }
        }

        static public void Submenu(Dictionary<Contact, Call[]> directory, Contact contact)
        {
            var exit = 0;
            Console.Clear();
            while (0 == exit)
            {
                Console.WriteLine("Upravlja se s kontaktom " + contact.Name + " s brojem " + contact.PhoneNumber + ":\n" +
                "1 - Ispis svih poziva s kontaktom\n" +
                "2 - Kreiranje novog poziva\n" +
                "0 - Povratak na glavni izbornik");

                int.TryParse(Console.ReadLine().Trim(), out int subchoice);

                switch (subchoice)
                {
                    case 1:
                        Console.Clear();
                        Contact.PrintCallsOfAContact(directory, contact.PhoneNumber);
                        break;
                    case 2:
                        Console.Clear();
                        //AddNewCall(directory, contact.PhoneNumber);
                        break;
                    case 0:
                        Console.Clear();
                        exit = 1;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Molimo unesite jedan od dopuštenih brojeva (0-2)");
                        break;
                }
            }
        }

        static public void ReturnToMenu()
        {
            Console.WriteLine("\nKliknite bilo koju tipku za povratak na izbornik.");
            Console.ReadKey();
            Console.Clear();
        }

        static void AddingDefaultContactsAndCalls(Dictionary<Contact, Call[]> directory)
        {
            var _contact = new Contact("Ante Antimon", "16161361", Preference.normal);
            var _call = new Call[] { new Call(new DateTime(1980, 11, 1, 23, 0, 1), CallStatus.complete),
                                      new Call(new DateTime(1986, 11, 1, 11, 11 ,11), CallStatus.complete),
                                      new Call(new DateTime(1970, 11, 1, 11, 11 ,11), CallStatus.complete)};
            directory.Add(_contact, _call);

            _contact = new Contact("Ivan Horvat", "25254333", Preference.favorite);
            _call = new Call[] { new Call(new DateTime(3030, 11, 1, 4, 4, 4), CallStatus.in_process) };
            directory.Add(_contact, _call);

            _contact = new Contact("Enver Hoxha", "6535633", Preference.blocked);
            _call = new Call[] { new Call(new DateTime(925, 11, 1, 22, 59, 59), CallStatus.missed) };
            directory.Add(_contact, _call);

            _contact = new Contact("Ivan Horvat", "7777777", Preference.favorite);
            directory.Add(_contact, null);

            _contact = new Contact("test", "1", Preference.favorite);
            directory.Add(_contact, null);
        }
    }
}
