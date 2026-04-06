using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using SalonUnghii.Models;

namespace SalonUnghii.App
{
    class Program
    {
        private const string FisierProgramari = "programari.txt";
        private const string FisierInventar = "inventar.txt";

        private static List<Programare> listaProgramari = new List<Programare>();
        private static List<Produs> inventar = new List<Produs>();
        private static readonly JsonSerializerOptions optiuniJson = new JsonSerializerOptions { WriteIndented = true };

        static void Main(string[] args)
        {
            IncarcaDate();

            bool ruleaza = true;
            while (ruleaza)
            {
                AfiseazaMeniu();
                string optiune = Console.ReadLine();

                switch (optiune)
                {
                    case "1": AdaugaProgramareNoua(); break;
                    case "2": AfiseazaToateProgramarile(); break;
                    case "3": CautaProgramareDupaNume(); break;
                    case "4": AfiseazaProgramarileDintrOData(); break;
                    case "5": AfiseazaInventarul(); break;
                    case "6": AdaugaProdusNou(); break;
                    case "7": ModificaCantitateProdus(); break;
                    case "8": StergeProgramare(); break; // Opțiune nouă
                    case "9":
                        ruleaza = false;
                        Console.WriteLine("Aplicatia se inchide. La revedere!");
                        break;
                    default:
                        Console.WriteLine("Optiune invalida!");
                        break;
                }
            }
        }

        private static void AfiseazaMeniu()
        {
            Console.WriteLine("\n=== MENIU SALON UNGHII ===");
            Console.WriteLine("1. Adauga programare");
            Console.WriteLine("2. Afiseaza toate programarile");
            Console.WriteLine("3. Cauta dupa nume clienta");
            Console.WriteLine("4. Afiseaza programari pe o data anume");
            Console.WriteLine("5. Afiseaza inventar");
            Console.WriteLine("6. Adauga produs nou");
            Console.WriteLine("7. Modifica stoc produs");
            Console.WriteLine("8. STERGE o programare");
            Console.WriteLine("9. Iesire");
            Console.Write("\nAlege: ");
        }

        // --- LOGICA PENTRU STERGE ---

        private static void StergeProgramare()
        {
            Console.WriteLine("\n--- STERGE O PROGRAMARE ---");
            Console.Write("Introdu ID-ul programarii pe care vrei sa o stergi: ");

            if (int.TryParse(Console.ReadLine(), out int idCautat))
            {
                // Cautam programarea in lista
                var programareDeSters = listaProgramari.FirstOrDefault(p => p.Id == idCautat);

                if (programareDeSters != null)
                {
                    Console.WriteLine($"Sigur stergi programarea pentru {programareDeSters.NumeClienta} din data {programareDeSters.Data:dd/MM/yyyy HH:mm}? (DA/NU)");
                    string confirmare = Console.ReadLine();

                    if (confirmare.ToUpper() == "DA")
                    {
                        listaProgramari.Remove(programareDeSters);
                        SalveazaProgramari(); // Actualizam fisierul text
                        Console.WriteLine("Programarea a fost stearsa cu succes!");
                    }
                    else
                    {
                        Console.WriteLine("Stergerea a fost anulata.");
                    }
                }
                else
                {
                    Console.WriteLine("Eroare: Nu s-a gasit nicio programare cu acest ID.");
                }
            }
            else
            {
                Console.WriteLine("Eroare: ID-ul trebuie sa fie un numar.");
            }
        }

       
        private static void IncarcaDate()
        {
            if (File.Exists(FisierProgramari))
            {
                string json = File.ReadAllText(FisierProgramari);
                listaProgramari = JsonSerializer.Deserialize<List<Programare>>(json) ?? new List<Programare>();
            }
            if (File.Exists(FisierInventar))
            {
                string json = File.ReadAllText(FisierInventar);
                inventar = JsonSerializer.Deserialize<List<Produs>>(json) ?? new List<Produs>();
            }
            else
            {
                // Produse default daca nu exista fisierul
                inventar = new List<Produs> { new Produs { Id = 1, Nume = "Baza", Cantitate = 10, Pret = 50m } };
                SalveazaInventar();
            }
        }

        private static void SalveazaProgramari()
        {
            File.WriteAllText(FisierProgramari, JsonSerializer.Serialize(listaProgramari, optiuniJson));
        }

        private static void SalveazaInventar()
        {
            File.WriteAllText(FisierInventar, JsonSerializer.Serialize(inventar, optiuniJson));
        }

        // --- ALTE METODE ---

        private static void AdaugaProgramareNoua()
        {
            Programare p = new Programare();
            p.Id = listaProgramari.Count > 0 ? listaProgramari.Max(x => x.Id) + 1 : 1;
            Console.Write("Nume clienta: "); p.NumeClienta = Console.ReadLine();
            Console.Write("Tip manichiura: "); p.TipManichiura = Console.ReadLine();
            Console.Write("Data (dd/mm/yyyy hh:mm): ");
            if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                p.Data = dt;
                listaProgramari.Add(p);
                SalveazaProgramari();
                Console.WriteLine("Salvat!");
            }
            else Console.WriteLine("Data invalida!");
        }

        private static void AfiseazaToateProgramarile()
        {
            Console.WriteLine("\n--- PROGRAMARI ---");
            foreach (var p in listaProgramari.OrderBy(x => x.Data))
                Console.WriteLine($"ID: {p.Id} | {p.Data:dd/MM HH:mm} | {p.NumeClienta} | {p.TipManichiura}");
        }

        private static void CautaProgramareDupaNume()
        {
            Console.Write("Nume exact: ");
            string nume = Console.ReadLine();
            var rez = listaProgramari.Where(p => p.NumeClienta.Equals(nume, StringComparison.OrdinalIgnoreCase));
            foreach (var p in rez) Console.WriteLine($"ID: {p.Id} | {p.Data}");
        }

        private static void AfiseazaProgramarileDintrOData()
        {
            Console.Write("Data (dd/mm/yyyy): ");
            if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                var rez = listaProgramari.Where(p => p.Data.Date == dt.Date);
                foreach (var p in rez) Console.WriteLine($"{p.Data:HH:mm} | {p.NumeClienta}");
            }
        }

        private static void AfiseazaInventarul()
        {
            foreach (var pr in inventar) Console.WriteLine($"{pr.Id}: {pr.Nume} - {pr.Cantitate} buc");
        }

        private static void AdaugaProdusNou()
        {
            Produs pr = new Produs();
            pr.Id = inventar.Count > 0 ? inventar.Max(x => x.Id) + 1 : 1;
            Console.Write("Nume: "); pr.Nume = Console.ReadLine();
            Console.Write("Cantitate: "); pr.Cantitate = int.Parse(Console.ReadLine());
            inventar.Add(pr);
            SalveazaInventar();
        }

        private static void ModificaCantitateProdus()
        {
            Console.Write("ID Produs: ");
            int id = int.Parse(Console.ReadLine());
            var pr = inventar.FirstOrDefault(x => x.Id == id);
            if (pr != null)
            {
                Console.Write("Noua cantitate: ");
                pr.Cantitate = int.Parse(Console.ReadLine());
                SalveazaInventar();
            }
        }
    }
}