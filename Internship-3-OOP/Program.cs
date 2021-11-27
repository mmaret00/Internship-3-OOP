using System;
using System.Collections.Generic;
using Internship_3_OOP.Entities;
using System.Threading;
using System.Linq;

namespace Internship_3_OOP
{
    class Program
    {
        public static Random RandomNumberGenerator = new();
        public static int RandomSeconds => RandomNumberGenerator.Next(5);
        public static int RandomCallStatus => RandomNumberGenerator.Next(2);

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
                        PrintContacts(directory, 0);
                        break;
                    case 2:
                        Console.Clear();
                        AddNewContact(directory);
                        break;
                    case 3:
                        Console.Clear();
                        PrintContacts(directory, 2);
                        DeleteContact(directory);
                        break;
                    case 4:
                        Console.Clear();
                        PrintContacts(directory, 2);
                        EditPreference(directory);
                        break;
                    case 5:
                        Console.Clear();
                        PrintContacts(directory, 2);
                        ManageContact(directory);
                        break;
                    case 6:
                        Console.Clear();
                        PrintContacts(directory, 1);
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
                        PrintCallsOfAContact(directory, contact.PhoneNumber);
                        break;
                    case 2:
                        Console.Clear();
                        CallAttempt(directory, contact);
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
            var _call = new Call[] { new Call(new DateTime(2018, 11, 1, 23, 0, 1), CallStatus.complete),
                                      new Call(new DateTime(2019, 11, 1, 11, 11 ,11), CallStatus.complete),
                                      new Call(new DateTime(2015, 11, 1, 11, 11 ,11), CallStatus.complete)};
            directory.Add(_contact, _call);

            _contact = new Contact("Ivan Horvat", "25254333", Preference.favorite);
            _call = new Call[] { new Call(new DateTime(2020, 11, 1, 4, 4, 4), CallStatus.missed) };
            directory.Add(_contact, _call);

            _contact = new Contact("Enver Hoxha", "6535633", Preference.blocked);
            _call = new Call[] { new Call(new DateTime(2018, 11, 1, 22, 59, 59), CallStatus.missed) };
            directory.Add(_contact, _call);

            _contact = new Contact("Ivan Horvat", "7777777", Preference.favorite);
            _call = new Call[] { new Call(new DateTime(2016, 11, 1, 22, 59, 59), CallStatus.in_process) };
            directory.Add(_contact, _call);

            _contact = new Contact("test", "1", Preference.favorite);
            _call = new Call[] { new Call(new DateTime(2017, 11, 1, 22, 59, 59), CallStatus.missed) };
            directory.Add(_contact, _call);
        }

        static public List<(DateTime, CallStatus)> CreateSortedListOfCalls(Call[] call)
        {
            var sortedCalls = new List<(DateTime, CallStatus)>();

            for (int i = 0; i < call.Length; i++)
            {
                sortedCalls.Add((call[i].CallSetupTime, call[i]._callStatus));
            }
            sortedCalls.Sort((a, b) => b.Item1.CompareTo(a.Item1));

            return sortedCalls;
        }

        static public void CallAttempt(Dictionary<Contact, Call[]> directory, Contact contact)
        {
            if (FindCallInProcess(directory))
            {
                var status = Program.RandomCallStatus;
                var _call = new Call((CallStatus)status);

                Console.Write("Pozivanje " + contact.Name + " na broj " + contact.PhoneNumber);
                WaitTime();

                if ((CallStatus)status == CallStatus.missed)
                {
                    Console.WriteLine("Kvrapcu, kontakt " + contact.Name + " se nije javio!");
                    Program.ReturnToMenu();
                }
                else if ((CallStatus)status == CallStatus.complete)
                {
                    CallTime();
                }

                directory[contact] = directory[contact].Concat(new Call[] { _call }).ToArray();
            }
        }

        static public bool FindCallInProcess(Dictionary<Contact, Call[]> directory)
        {
            foreach (var i in directory)
            {
                foreach (var j in i.Value)
                {
                    if (j._callStatus == CallStatus.in_process)
                    {
                        Console.WriteLine("U tijeku je poziv s kontaktom " + i.Key.Name + " (broj " + i.Key.PhoneNumber + "), želite li ga prekinuti? Ako želite, upišite 'da':");
                        var endCallChoice = Console.ReadLine();
                        if ("da" == endCallChoice)
                        {
                            j._callStatus = CallStatus.complete;
                            return true;
                        }
                        else
                        {
                            Program.ReturnToMenu();
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        static public void WaitTime()
        {
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.Clear();
        }

        static public void CallTime()
        {
            int duration = Program.RandomSeconds;
            Console.WriteLine("Poziv u trajanju od " + duration + " sek. u tijeku:");
            for (int i = 1; i <= duration; i++)
            {
                Console.Write("\r{0:0}. sekunda razgovora", i);
                Thread.Sleep(1000);
            }
            Console.WriteLine("\nRazgovor uspješno završen.");
            Program.ReturnToMenu();
        }

        static public void AddNewContact(Dictionary<Contact, Call[]> directory)
        {
            Console.WriteLine("Unesi ime novog kontakta:");
            var name = Console.ReadLine().Trim();
            if (0 == name.Length)
            {
                Console.WriteLine("Nedopušten je unos praznog imena!");
                ReturnToMenu();
                return;
            }

            Console.WriteLine("Unesi broj:");
            var number = CheckIfNumberIsTaken(directory);
            if(null == number)
            {
                return;
            }

            Preference prefer = GetValidPreference();

            var _contact = new Contact(name, number, prefer);
            directory.Add(_contact, null);
            Console.Clear();
            Console.WriteLine("Unešen je kontakt " + name + " s brojem " + number + " i preferencom " + prefer + ".");
            ReturnToMenu();
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
            var exit = 0;
            var number = "";
            while (0 == exit)
            {
                number = Console.ReadLine().Trim();
                if (0 == number.Length)
                {
                    Console.WriteLine("Nedopušten je unos praznog broja!");
                    ReturnToMenu();
                    return null;
                }

                var exists = 0;
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
                    Console.WriteLine("Kontakt " + deleteName + " s brojem " + deleteNumber + " je obrisan.");
                }
            }
            ReturnToMenu();
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
                        Preference newPreference = GetValidPreference();
                        item.Key.Pref = newPreference;
                        Console.WriteLine("Preferenca je promijenjena u " + newPreference + ".");
                    }
                }

            if (0 == exists)
            {
                Console.WriteLine("Ne postoji kontakt s brojem " + changeNumber + ".");
            }
            ReturnToMenu();
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

            if (0 == exists)
            {
                Console.WriteLine("Broj ne postoji u imeniku.");
                ReturnToMenu();
            }
            else if (1 == exists)
            {
                Submenu(directory, contactToManage);
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
                ReturnToMenu();
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
                        var sortedCalls = CreateSortedListOfCalls(item.Value);

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
            ReturnToMenu();
        }
    }
}
