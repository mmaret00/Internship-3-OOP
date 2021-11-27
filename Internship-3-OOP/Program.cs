using System;
using System.Collections.Generic;
using Internship_3_OOP.Entities;
using System.Threading;
using System.Linq;

namespace Internship_3_OOP
{
    enum PrintChoice
    {
        ShowCalls,
        ShowOnlyContacts,
        ShowContactsForOtherMethods,
        ShowCallsForOnlyOneContact
    }

    class Program
    {
        static Random RandomNumberGenerator = new();
        static int RandomSeconds => RandomNumberGenerator.Next(20);
        static int RandomCallStatus => RandomNumberGenerator.Next(2);

        static void Main()
        {
            var directory = new Dictionary<Contact, Call[]>() { };
            AddingDefaultContactsAndCalls(directory);
            Menu(directory);
        }

        static void Menu(Dictionary<Contact, Call[]> directory)
        {
            var exitMenu = false;
            while (true != exitMenu)
            {
                Console.WriteLine("Odaberite akciju:\n" +
                "1 - Ispis svih kontakata\n" +
                "2 - Dodavanje novog kontakta u imenik\n" +
                "3 - Brisanje kontakta iz imenika\n" +
                "4 - Promjena preferencije kontakta\n" +
                "5 - Upravljanje kontaktom\n" +
                "6 - Ispis svih poziva\n" +
                "0 - Izlaz iz aplikacije");

                char.TryParse(Console.ReadLine().Trim(), out char choice);

                switch (choice)
                {
                    case '1':
                        Console.Clear();
                        Print(directory, 0);
                        break;
                    case '2':
                        Console.Clear();
                        AddNewContact(directory);
                        break;
                    case '3':
                        Console.Clear();
                        if (Print(directory, (PrintChoice)2))
                        {
                            DeleteContact(directory);
                        }
                        break;
                    case '4':
                        Console.Clear();
                        if (Print(directory, (PrintChoice)2))
                        {
                            EditPreference(directory);
                        }
                        break;
                    case '5':
                        Console.Clear();
                        if (Print(directory, (PrintChoice)2))
                        {
                            ManageContact(directory);
                        }
                        break;
                    case '6':
                        Console.Clear();
                        Print(directory, (PrintChoice)1);
                        break;
                    case '0':
                        Console.Clear();
                        exitMenu = true;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Molimo unesite jedan od dopuštenih brojeva (0-6)\n");
                        break;
                }
            }
        }

        static void Submenu(Dictionary<Contact, Call[]> directory, Contact contact)
        {
            var exitMenu = false;
            Console.Clear();
            while (false == exitMenu)
            {
                Console.WriteLine($"Upravlja se s kontaktom {contact.Name} s brojem {contact.PhoneNumber}:\n" +
                "1 - Ispis svih poziva s kontaktom\n" +
                "2 - Kreiranje novog poziva\n" +
                "0 - Povratak na glavni izbornik");

                char.TryParse(Console.ReadLine().Trim(), out char subchoice);

                switch (subchoice)
                {
                    case '1':
                        Console.Clear();
                        PrintContacts(directory, (PrintChoice)4, contact.PhoneNumber);
                        break;
                    case '2':
                        Console.Clear();
                        CallAttempt(directory, contact);
                        break;
                    case '0':
                        Console.Clear();
                        exitMenu = true;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Molimo unesite jedan od dopuštenih brojeva (0-2)\n");
                        break;
                }
            }
        }

        static void ReturnToMenu()
        {
            Console.WriteLine("\nKliknite bilo koju tipku za povratak na izbornik.");
            Console.ReadKey();
            Console.Clear();
        }

        static void AddingDefaultContactsAndCalls(Dictionary<Contact, Call[]> directory)
        {
            var _contact = new Contact("Ante Antimon", "0931616136", Preference.Normal);
            var _call = new Call[] { new Call(new DateTime(2018, 11, 1, 23, 0, 1), CallStatus.Complete),
                                      new Call(new DateTime(2019, 11, 1, 11, 11 ,11), CallStatus.Missed),
                                      new Call(new DateTime(2015, 11, 1, 11, 11 ,11), CallStatus.Complete)};
            directory.Add(_contact, _call);

            _contact = new Contact("Ivan Horvat", "0902525433", Preference.Normal);
            _call = new Call[] { new Call(new DateTime(2020, 11, 1, 4, 4, 4), CallStatus.Missed),
                                 new Call(new DateTime(2021, 11, 1, 4, 4, 4), CallStatus.Complete)};
            directory.Add(_contact, _call);

            _contact = new Contact("Enver Hoxha", "965357633", Preference.Blocked);
            _call = new Call[] { new Call(new DateTime(2018, 12, 1, 22, 59, 59), CallStatus.Missed),
                                 new Call(new DateTime(2020, 12, 1, 4, 4, 4), CallStatus.Complete),
                                 new Call(new DateTime(2019, 12, 1, 4, 4, 4), CallStatus.Complete)};
            directory.Add(_contact, _call);

            _contact = new Contact("Ivan Horvat", "977777777", Preference.Favorite);
            _call = new Call[] { new Call(new DateTime(2016, 11, 1, 22, 59, 59), CallStatus.In_process) };
            directory.Add(_contact, _call);

            _contact = new Contact("Brat Stipan", "0961234567", Preference.Favorite);
            directory.Add(_contact, null);
        }

        static List<(DateTime, CallStatus)> CreateSortedListOfCalls(Call[] call)
        {
            var sortedCalls = new List<(DateTime, CallStatus)>();

            for (int i = 0; i < call.Length; i++)
            {
                sortedCalls.Add((call[i].CallSetupTime, call[i]._callStatus));
            }
            sortedCalls.Sort((a, b) => b.Item1.CompareTo(a.Item1));

            return sortedCalls;
        }

        static void CallAttempt(Dictionary<Contact, Call[]> directory, Contact contact)
        {
            if (!FindCallInProcess(directory) || !CheckIfNumberIsBlocked(contact))
            {
                return;
            }
            
            var status = Program.RandomCallStatus;
            Call _call = null;

            Console.Write($"Pozivanje {contact.Name} na broj {contact.PhoneNumber}");
            WaitTime();

            if ((CallStatus)status == (CallStatus)0)
            {
                _call = new Call((CallStatus)status);
                Console.WriteLine($"Kvrapcu, kontakt {contact.Name} se nije javio!");
                Program.ReturnToMenu();
            }
            else if ((CallStatus)status == (CallStatus)1)
            {
                _call = new Call((CallStatus)status);
                CallTime();
            }
            AddNewCall(directory, contact, _call);
        }

        static void AddNewCall(Dictionary<Contact, Call[]> directory, Contact _contact, Call _call)
        {
            foreach (var contact in directory)
            {
                if (contact.Key == _contact)
                {
                    if (null != contact.Value)
                    {
                        directory[_contact] = directory[_contact].Concat(new Call[] { _call }).ToArray();
                    }
                    else
                    {
                        directory[_contact] = new Call[] { _call };
                    }
                }
            }
        }

        static bool CheckIfNumberIsBlocked(Contact contact)
        {
            if (contact.Pref == Preference.Blocked)
            {
                Console.WriteLine($"Ne možete nazvati kontakt {contact.Name} (broj: {contact.PhoneNumber}) jer je blokiran.");
                ReturnToMenu();
                return false;
            }
            else return true;
        }

        static bool FindCallInProcess(Dictionary<Contact, Call[]> directory)
        {
            foreach (var i in directory)
            {
                if (null == i.Value) continue;
                foreach (var j in i.Value)
                {
                    if (j._callStatus != CallStatus.In_process) continue;
                    
                    Console.WriteLine($"U tijeku je poziv s kontaktom {i.Key.Name} (broj {i.Key.PhoneNumber}), želite li ga prekinuti? Ako želite, upišite 'da':");
                    var endCallChoice = Console.ReadLine().Trim().ToUpper();
                    if ("DA" == endCallChoice)
                    {
                        j._callStatus = (CallStatus)1;
                        return true;
                    }
                    else
                    {
                        Program.ReturnToMenu();
                        return false;
                    }
                }
            }
            return true;
        }

        static void WaitTime()
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

        static void CallTime()
        {
            int duration = Program.RandomSeconds;
            Console.WriteLine($"Poziv u trajanju od {duration} sek. u tijeku:");
            for (int i = 1; i <= duration; i++)
            {
                Console.Write("\r{0:0}. sekunda razgovora", i);
                Thread.Sleep(1000);
            }
            Console.WriteLine("\nRazgovor uspješno završen.");
            Program.ReturnToMenu();
        }

        static bool CheckIfNameIsEmpty(string name)
        {
            if (0 == name.Length)
            {
                Console.WriteLine("Nedopušten je unos praznog imena!");
                ReturnToMenu();
                return false;
            }
            return true;
        }

        static void AddNewContact(Dictionary<Contact, Call[]> directory)
        {
            Console.WriteLine("Unesi ime novog kontakta:");
            var name = Console.ReadLine();
            if (!CheckIfNameIsEmpty(name))
            {
                return;
            }
            
            Console.WriteLine("Unesi broj:");
            var number = CheckIfNumberIsTaken(directory);
            if(null == number)
            {
                return;
            }

            var newPref = GetValidPreference();
            if (-1 == newPref)
            {
                return;
            }
            Preference prefer = (Preference)newPref;

            var _contact = new Contact(name, number, prefer);
            directory.Add(_contact, null);
            Console.Clear();
            Console.WriteLine($"Unešen je kontakt {name} s brojem {number} i preferencijom {prefer}.");
            ReturnToMenu();
        }

        static int GetValidPreference()
        {
            Console.WriteLine("Unesi preferenciju (1 - favorite, 2 - normal, 3 - blocked):");
            var choice = Console.ReadLine().Trim();

            switch (choice)
            {
                case "1":
                case "favorite":
                    return 0;
                case "2":
                case "normal":
                    return 1;
                case "3":
                case "blocked":
                    return 2;
                default:
                    Console.WriteLine("Nedopušten unos, unesite broj (1-3) ili jednu od tri ponuđene riječi.");
                    ReturnToMenu();
                    return -1;
            }
        }

        static string CheckIfNumberIsTaken(Dictionary<Contact, Call[]> directory)
        {
            var number = GetValidPhoneNumber();
            if (null == number)
            {
                return null;
            }

            foreach (var contact in directory)
            {
                if (number != contact.Key.PhoneNumber) continue;
                
                Console.WriteLine("Taj broj već postoji u imeniku!:");
                ReturnToMenu();
                return null;
            }
            return number;
        }

        static bool NumberDigitsCheck(string number)
        {
            foreach (var digit in number)
            {
                if (!char.IsDigit(digit))
                {
                    return false;
                }
            }
            return true;
        }

        static string GetValidPhoneNumber()
        {
            string findNumber = (Console.ReadLine().Trim());

            if (CheckIfNumberIsEmpty(findNumber) && CheckIfNumberIsNumeric(findNumber))
            {
                return findNumber;
            }
            return null;
        }

        static bool CheckIfNumberIsNumeric(string number)
        {
            if (!NumberDigitsCheck(number))
            {
                Console.WriteLine("Broj treba sadržavati samo brojeve!");
                ReturnToMenu();
                return false;
            }
            return true;
        }

        static bool CheckIfNumberIsEmpty(string number)
        {
            if (0 == number.Length)
            {
                Console.WriteLine("Nedopušten je unos praznog broja!");
                ReturnToMenu();
                return false;
            }
            return true;
        }

        static void DeleteContact(Dictionary<Contact, Call[]> directory)
        {
            Console.WriteLine("Unesi broj kontakta kojeg želiš obrisati: ");
            var deleteNumber = GetValidPhoneNumber();
            if (null == deleteNumber)
            {
                return;
            }

            Contact contactToDelete = CheckIfDirectoryContainsNumber(directory, deleteNumber);
            if(null != contactToDelete)
            {
                var deleteName = contactToDelete.Name;
                directory.Remove(contactToDelete);
                Console.WriteLine($"Kontakt {deleteName} s brojem {deleteNumber} je obrisan.");
                ReturnToMenu();
            }
        }

        static Contact CheckIfDirectoryContainsNumber(Dictionary<Contact, Call[]> directory, string number)
        {
            foreach (var contact in directory)
            {
                if (number != contact.Key.PhoneNumber) continue;

                return contact.Key;
            }

            Console.WriteLine("Imenik ne sadrži taj kontakt.");
            ReturnToMenu();
            return null;
        }

        static void EditPreference(Dictionary<Contact, Call[]> directory)
        {
            Console.WriteLine("Unesi broj kontakta kojem želiš promijeniti preferenciju:");
            string changeNumber = GetValidPhoneNumber();
            if (null == changeNumber)
            {
                return;
            }

            Contact contactToChange = CheckIfDirectoryContainsNumber(directory, changeNumber);
            if (null != contactToChange)
            {
                Console.WriteLine("Unesi novu preferenciju:");
                var newPref = GetValidPreference();
                if (-1 == newPref)
                {
                    return;
                }
                contactToChange.Pref = (Preference)newPref;
                Console.WriteLine($"Preferencija je promijenjena u {(Preference)newPref}.");
                ReturnToMenu();
            }
        }

        static void ManageContact(Dictionary<Contact, Call[]> directory)
        {
            Console.WriteLine("Unesite broj kontakta:");
            var number = GetValidPhoneNumber();
            if(null == number)
            {
                return;
            }

            Contact contactToManage = CheckIfDirectoryContainsNumber(directory, number);
            if (null != contactToManage)
            {
                Submenu(directory, contactToManage);
            }
        }

        static bool Print(Dictionary<Contact, Call[]> directory, PrintChoice choice)
        {
            if (0 == directory.Count)
            {
                Console.WriteLine("Imenik je prazan!");
                ReturnToMenu();
                return false;
            }
            if ((PrintChoice)1 != choice)
            {
                Console.WriteLine("Kontakti:");
            }
            PrintContacts(directory, choice, null);

            if (0 == choice || (PrintChoice)1 == choice)
            {
                ReturnToMenu();
            }
            return true;
        }

        static void PrintContacts(Dictionary<Contact, Call[]> directory, PrintChoice choice, string number)
        {
            foreach (var contact in directory)
            {
                if ((PrintChoice)4 == choice && number != contact.Key.PhoneNumber) continue;
                
                Console.WriteLine($"{ contact.Key.Name}\t broj: { contact.Key.PhoneNumber}\t preferencija: { contact.Key.Pref}");
                
                if((PrintChoice)4 == choice && null != contact.Value)
                {
                    var sortedCalls = CreateSortedListOfCalls(contact.Value);
                    PrintCallsFromAList(sortedCalls);
                }
                
                if ((PrintChoice)1 == choice && null != contact.Value)
                {
                    PrintCalls(contact.Value);
                }
                else if (((PrintChoice)1 == choice || (PrintChoice)4 == choice) && contact.Value == null)
                {
                    Console.WriteLine("\nNema poziva s kontaktom.");
                    if((PrintChoice)1 == choice) Console.WriteLine("\n=================================================\n");
                }
            }
            if ((PrintChoice)4 == choice) ReturnToMenu();
        }
        static void PrintCallsFromAList(List<(DateTime, CallStatus)> list)
        {
            Console.WriteLine("\nDatum i vrijeme poziva:\t\tStatus:");
            for (int i = 0; i < list.Count; i++)
                Console.WriteLine($"{list[i].Item1}\t\t{list[i].Item2}");
        }

        static void PrintCalls(Call[] call)
        {
            Console.WriteLine("\nDatum i vrijeme poziva:\t\tStatus:");
            for (int i = 0; i < call.Length; i++)
                Console.WriteLine($"{call[i].CallSetupTime}\t\t{call[i]._callStatus}");
            Console.WriteLine("\n=================================================\n");
        }
    }
}
