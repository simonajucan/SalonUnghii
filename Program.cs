using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SalonUnghii.Models;

namespace SalonUnghii.App
{
    class Program
    {
        static void Main(string[] args)
        {
            // Lista pentru programari
            List<Programare> listaProgramari = new List<Programare>();

            // Lista pentru inventarul de produse (pre-populata cu produsele tale)
            List<Produs> inventar = new List<Produs>
            {
                new Produs { Id = 1, Nume = "Baza", Cantitate = 10, Pret = 50.0m },
                new Produs { Id = 2, Nume = "Primer", Cantitate = 15, Pret = 30.0m },
                new Produs { Id = 3, Nume = "Bonder", Cantitate = 12, Pret = 35.0m },
                new Produs { Id = 4, Nume = "Gel constructie", Cantitate = 8, Pret = 80.0m },
                new Produs { Id = 5, Nume = "Top Coat", Cantitate = 20, Pret = 45.0m }
            };

            bool ruleaza = true;

            while (ruleaza)
            {
                Console.WriteLine("\n=== MENIU SALON UNGHII ===");
                Console.WriteLine("--- PROGRAMARI ---");
                Console.WriteLine("1. Adauga o programare noua");
                Console.WriteLine("2. Afiseaza toate programarile");
                Console.WriteLine("3. Cauta o programare dupa nume clienta");
                Console.WriteLine("4. Afiseaza programarile dintr-o anumita data");
                Console.WriteLine("--- INVENTAR PRODUSE ---");
                Console.WriteLine("5. Afiseaza inventarul de produse");
                Console.WriteLine("6. Adauga un produs nou in inventar");
                Console.WriteLine("7. Modifica cantitatea unui produs");
                Console.WriteLine("--- SISTEM ---");
                Console.WriteLine("8. Iesire");
                Console.Write("\nAlege o optiune: ");

                string optiune = Console.ReadLine();

                switch (optiune)
                {
                    // --- ZONA PROGRAMARI ---
                    case "1":
                        Programare pNoua = new Programare();
                        pNoua.Id = listaProgramari.Count > 0 ? listaProgramari.Max(p => p.Id) + 1 : 1;

                        Console.Write("Nume clienta: ");
                        pNoua.NumeClienta = Console.ReadLine();

                        Console.Write("Tip manichiura: ");
                        pNoua.TipManichiura = Console.ReadLine();

                        Console.Write("Data si ora (format zz/ll/aaaa oo:mm, ex: 30/03/2026 13:00): ");
                        string dataInput = Console.ReadLine();

                        if (DateTime.TryParseExact(dataInput, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataProgramare))
                        {
                            if (dataProgramare <= DateTime.Now)
                            {
                                Console.WriteLine("Eroare: Programarea trebuie sa fie facuta in viitor!");
                                break;
                            }

                            // Verificare suprapunere programari (optional, pentru o mecanica extra)
                            bool oraOcupata = listaProgramari.Any(p => Math.Abs((p.Data - dataProgramare).TotalMinutes) < 60);
                            if (oraOcupata)
                            {
                                Console.WriteLine("Atentie: Exista deja o programare in acest interval de timp!");
                                break;
                            }

                            pNoua.Data = dataProgramare;
                            listaProgramari.Add(pNoua);
                            Console.WriteLine("Programare salvata cu succes!");
                        }
                        else
                        {
                            Console.WriteLine("Eroare: Formatul datei este invalid.");
                        }
                        break;

                    case "2":
                        Console.WriteLine("\n--- LISTA PROGRAMARI ---");
                        if (listaProgramari.Count == 0) Console.WriteLine("Nu exista programari.");
                        else
                        {
                            foreach (var prog in listaProgramari.OrderBy(p => p.Data))
                            {
                                Console.WriteLine($"ID: {prog.Id} | Data: {prog.Data:dd/MM/yyyy HH:mm} | Clienta: {prog.NumeClienta} | Tip: {prog.TipManichiura}");
                            }
                        }
                        break;

                    case "3":
                        Console.Write("\nIntrodu numele clientei pe care o cauti: ");
                        string numeCautat = Console.ReadLine();
                        var rezultate = listaProgramari.Where(p => p.NumeClienta.Contains(numeCautat, StringComparison.OrdinalIgnoreCase)).ToList();

                        if (rezultate.Count > 0)
                        {
                            foreach (var prog in rezultate)
                                Console.WriteLine($"ID: {prog.Id} | Data: {prog.Data:dd/MM/yyyy HH:mm} | Tip: {prog.TipManichiura}");
                        }
                        else Console.WriteLine("Nu s-a gasit nicio programare.");
                        break;

                    case "4":
                        Console.Write("\nIntrodu data cautata (format zz/ll/aaaa, ex: 30/03/2026): ");
                        string dataCautareInput = Console.ReadLine();

                        if (DateTime.TryParseExact(dataCautareInput, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataCautata))
                        {
                            var programariPeData = listaProgramari.Where(p => p.Data.Date == dataCautata.Date).OrderBy(p => p.Data).ToList();
                            if (programariPeData.Count > 0)
                            {
                                Console.WriteLine($"\n--- PROGRAMARI PENTRU {dataCautata:dd/MM/yyyy} ---");
                                foreach (var prog in programariPeData)
                                    Console.WriteLine($"Ora: {prog.Data:HH:mm} | ID: {prog.Id} | Clienta: {prog.NumeClienta} | Tip: {prog.TipManichiura}");
                            }
                            else Console.WriteLine("Nu exista nicio programare in aceasta data.");
                        }
                        else Console.WriteLine("Eroare: Format data invalid.");
                        break;

                    // --- ZONA INVENTAR ---
                    case "5":
                        Console.WriteLine("\n--- INVENTAR PRODUSE ---");
                        if (inventar.Count == 0) Console.WriteLine("Inventarul este gol.");
                        else
                        {
                            foreach (var produs in inventar)
                            {
                                Console.WriteLine($"ID: {produs.Id} | Nume: {produs.Nume} | Cantitate: {produs.Cantitate} buc. | Pret: {produs.Pret} RON");
                            }
                        }
                        break;

                    case "6":
                        Produs produsNou = new Produs();
                        produsNou.Id = inventar.Count > 0 ? inventar.Max(p => p.Id) + 1 : 1;

                        Console.Write("Nume produs nou: ");
                        produsNou.Nume = Console.ReadLine();

                        Console.Write("Cantitate initiala: ");
                        if (int.TryParse(Console.ReadLine(), out int cantitate)) produsNou.Cantitate = cantitate;
                        else produsNou.Cantitate = 0;

                        Console.Write("Pret (ex: 45.50): ");
                        if (decimal.TryParse(Console.ReadLine(), CultureInfo.InvariantCulture, out decimal pret)) produsNou.Pret = pret;
                        else produsNou.Pret = 0;

                        inventar.Add(produsNou);
                        Console.WriteLine($"Produsul '{produsNou.Nume}' a fost adaugat cu succes in inventar!");
                        break;

                    case "7":
                        Console.WriteLine("\n--- MODIFICARE CANTITATE PRODUS ---");
                        Console.Write("Introdu ID-ul produsului pe care vrei sa il modifici: ");
                        if (int.TryParse(Console.ReadLine(), out int idProdusCautat))
                        {
                            // Cautam produsul in lista
                            Produs produsGasit = inventar.FirstOrDefault(p => p.Id == idProdusCautat);

                            if (produsGasit != null)
                            {
                                Console.WriteLine($"Ai selectat: {produsGasit.Nume} (Cantitate curenta: {produsGasit.Cantitate})");
                                Console.Write("Introdu noua cantitate: ");

                                if (int.TryParse(Console.ReadLine(), out int cantitateNoua))
                                {
                                    produsGasit.Cantitate = cantitateNoua;
                                    Console.WriteLine($"Cantitatea a fost actualizata! Noul stoc pentru {produsGasit.Nume} este de {produsGasit.Cantitate} buc.");
                                }
                                else Console.WriteLine("Eroare: Cantitatea trebuie sa fie un numar intreg.");
                            }
                            else Console.WriteLine("Eroare: Nu s-a gasit niciun produs cu acest ID.");
                        }
                        else Console.WriteLine("Eroare: ID invalid.");
                        break;

                    case "8":
                        ruleaza = false;
                        Console.WriteLine("Aplicatia se inchide. La revedere!");
                        break;

                    default:
                        Console.WriteLine("Optiune invalida! Te rog sa alegi un numar din meniu.");
                        break;
                }
            }
        }
    }
}