//zasad su svi public, prominit to kasnije

using System;
using System.Collections.Generic;

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

    public enum CallStatus
    {
        in_process,
        missed,
        complete
    }

    public class Call
    {
        public DateTime CallSetupTime { get; set; }
        public CallStatus _callStatus;

        public Call()
        {
            CallSetupTime = DateTime.Now;
            _callStatus = CallStatus.complete;
        }

        public Call(DateTime dt, CallStatus cs)
        {
            CallSetupTime = dt;
            _callStatus = cs;
        }
    }

    class Program
    {
        static void Main()
        {
            var directory = new Dictionary<Contact, Call[]>() { };
            AddingDefault(directory);
            int choice, exit = 0;

            while (1 != exit)
            {
                Console.WriteLine("\nOdaberite akciju:");
                Console.WriteLine("1 - Ispis svih kontakata");
                Console.WriteLine("2 - Dodavanje novog kontakta u imenik");
                Console.WriteLine("3 - Brisanje kontakta iz imenika");
                Console.WriteLine("4 - Promjena preference kontakta");
                //Console.WriteLine("5. Upravljanje kontaktom koje otvara podmenu sa sljedećim funkcionalnostima:");
                Console.WriteLine("6 - Ispis svih poziva");
                Console.WriteLine("0 - Izlaz iz aplikacije");

                int.TryParse(Console.ReadLine().Trim(), out choice);

                switch (choice)
                {
                    case 1:
                        PrintContacts(directory, 0);
                        break;
                    case 2:
                        AddNewContact(directory);
                        break;
                    case 3:
                        Delete_Contact(directory);
                        break;
                    case 4:
                        Edit_Preference(directory);
                        break;
                    case 6:
                        PrintContacts(directory, 1);
                        break;
                    case 0:
                        exit = 1;
                        break;
                    default:
                        break;
                }
            }
        }

        static void PrintContacts(Dictionary<Contact, Call[]> directory, int choice)
        {
            Console.WriteLine("\nKontakti:");

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
            }
        }

        static void AddNewContact(Dictionary<Contact, Call[]> directory)
        {
            Console.WriteLine("Unesi ime novog kontakta:");
            var name = Console.ReadLine();

            Console.WriteLine("Unesi broj:");
            var number = Phone_Number_Check();

            Preference prefer = Preference_Check();

            var _contact = new Contact(name, number, prefer);
            directory.Add(_contact, null);
        }

        static Preference Preference_Check()
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

        static void Delete_Contact(Dictionary<Contact, Call[]> directory)
        {
            Console.WriteLine("Unesi broj kontakta kojeg želiš obrisati: ");
            string deleteNumber = Phone_Number_Check();
            string confirmation;
            int correct = 0;
            Contact contactToDelete = null;
            var deleteName = "";

            int exists = 0;

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
                while (0 == correct)
                {
                    Console.WriteLine("\nJeste li sigurni da želite obrisati kontakt " + contactToDelete.Name + " s brojem " + deleteNumber + "?");
                    Console.WriteLine("(da/ne)");
                    confirmation = Console.ReadLine().Trim();
                    if ("da" == confirmation)
                    {
                        correct = 1;
                        deleteName = contactToDelete.Name;
                        directory.Remove(contactToDelete);
                        Console.WriteLine("Stanovnik " + deleteName + " s brojem " + deleteNumber + " je obrisan.");
                    }
                    else if ("ne" == confirmation)
                    {
                        correct = 1;
                        Console.WriteLine("Povratak na glavni izbornik.");
                    }
                    else
                        Console.WriteLine("Nepravilan unos, molimo ponovite (unesite 'da' ili 'ne'):");
                }
            }
        }

        static void Edit_Preference(Dictionary<Contact, Call[]> directory)
        {
            Console.WriteLine("Unesi ime kontakta kojem želiš promijeniti preferencu:");
            string changeName = Console.ReadLine();
            Console.WriteLine("Unesi broj tog kontakta:");
            string changeNumber = Phone_Number_Check();
            string confirmation;
            int correct = 0, exists = 0;

            foreach (var item in directory)
                if (changeNumber == item.Key.PhoneNumber && changeName == item.Key.Name)
                {
                    exists++;
                    while (0 == correct)
                    {
                        Console.WriteLine("\nJeste li sigurni da želite promijeniti preferencu kontakta " + item.Key.Name + " s brojem " + item.Key.PhoneNumber + "?");
                        Console.WriteLine("(da/ne)");
                        confirmation = Console.ReadLine().Trim();
                        if ("da" == confirmation)
                        {
                            correct = 1;
                            Console.WriteLine("Unesi novu preferencu:");
                            Preference newPreference = Preference_Check();
                            item.Key.Pref = newPreference;
                            Console.WriteLine("Preferenca je promijenjena u " + newPreference + ".");
                        }
                        else if ("ne" == confirmation)
                        {
                            correct = 1;
                            Console.WriteLine("Povratak na glavni izbornik.");
                        }
                        else
                            Console.WriteLine("Nepravilan unos, molimo ponovite (unesite 'da' ili 'ne'):");
                    }
                }

            if(0 == exists) Console.WriteLine("Ne postoji kontakt " + changeName + " s brojem " + changeNumber + ".");
        }

        static string Phone_Number_Check()
        {
            int repeat;
            string findNumber;

            do
            {
                repeat = 0;
                findNumber = (Console.ReadLine().Trim());

                if (!Number_Digits_Check(findNumber))
                {
                    Console.WriteLine("Broj treba sadržavati samo brojeve! Unesite ga opet:");
                    repeat = 1;
                }

            } while (0 != repeat);

            return findNumber;
        }

        static bool Number_Digits_Check(string number)
        {
            foreach (var item in number)
                if (item < '0' || item > '9')
                    return false;

            return true;
        }

        static void AddingDefault(Dictionary<Contact, Call[]> directory)
        {
            var _contact = new Contact("Ante Antimon", "16161361", Preference.normal);
            var _call = new Call[] { new Call(new DateTime(1980, 11, 1, 23, 0, 1), CallStatus.complete),
                                      new Call(new DateTime(1986, 11, 1, 11, 11 ,11), CallStatus.complete)};
            directory.Add(_contact, _call);

            _contact = new Contact("Ivan Horvat", "25254333", Preference.favorite);
            _call = new Call[] { new Call(new DateTime(3030, 11, 1, 4, 4, 4), CallStatus.in_process) };
            directory.Add(_contact, _call);

            _contact = new Contact("Enver Hoxha", "6535633", Preference.blocked);
            _call = new Call[] { new Call(new DateTime(925, 11, 1, 22, 59, 59), CallStatus.missed) };
            directory.Add(_contact, _call);

            _contact = new Contact("Ivan Horvat", "7777777", Preference.favorite);
            directory.Add(_contact, null);
        }

        static Call AddNewCall() //ovo cu kasnije
        {
            Console.WriteLine("Unesi ime osobe kojoj dodajes novi poziv:");
            var contactName = Console.ReadLine();

            Console.WriteLine("Unesi pocetak trajanja:");
            var dateOfBirth = DateTime.Parse(Console.ReadLine().Trim());

            Console.WriteLine("Unesi status:");
            var pref = Console.ReadLine();

            var _call = new Call(dateOfBirth, CallStatus.complete);//dodat za status kasnije

            return _call;
        }
    }
}
