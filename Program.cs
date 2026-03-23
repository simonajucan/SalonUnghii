using System;
using System.Collections.Generic;
using System.Linq;
// Aici importăm clasele din proiectul Class Library
using SalonUnghii.Models;

namespace SalonUnghii.App
{
    class Program
    {
        static void Main(string[] args)
        {
            // Inițializăm lista în care vom salva datele
            List<Programare> listaProgramari = new List<Programare>();
            bool ruleaza = true;

            while (ruleaza)
            {
                Console.WriteLine("\n--- MENIU SALON UNGHII ---");
                Console.WriteLine("1. Adaugă o programare nouă");
                Console.WriteLine("2. Afișează toate programările");
                Console.WriteLine("3. Caută o programare");
                Console.WriteLine("4. Ieșire");
                Console.Write("Alege o opțiune: ");

                string optiune = Console.ReadLine();

                switch (optiune)
                {
                    case "1":
                        // 1. Citirea și salvarea datelor
                        Programare pNoua = new Programare();
                        pNoua.Id = listaProgramari.Count + 1;

                        Console.Write("Nume clientă: ");
                        pNoua.NumeClienta = Console.ReadLine();

                        Console.Write("Tip manichiură: ");
                        pNoua.TipManichiura = Console.ReadLine();

                        pNoua.Data = DateTime.Now; // Setăm data curentă automat pentru simplitate

                        listaProgramari.Add(pNoua);
                        Console.WriteLine("Programare salvată cu succes!");
                        break;

                    case "2":
                        // 2. Afișarea datelor din colecție
                        Console.WriteLine("\n--- LISTĂ PROGRAMĂRI ---");
                        if (listaProgramari.Count == 0)
                        {
                            Console.WriteLine("Nu există programări.");
                        }
                        else
                        {
                            foreach (var prog in listaProgramari)
                            {
                                Console.WriteLine($"ID: {prog.Id} | Clientă: {prog.NumeClienta} | Tip: {prog.TipManichiura}");
                            }
                        }
                        break;

                    case "3":
                        // 3. Căutarea după criterii
                        Console.Write("\nIntrodu numele clientei pe care o cauți: ");
                        string numeCautat = Console.ReadLine();

                        var rezultate = listaProgramari.Where(p => p.NumeClienta.Contains(numeCautat, StringComparison.OrdinalIgnoreCase)).ToList();

                        if (rezultate.Count > 0)
                        {
                            Console.WriteLine($"S-au găsit {rezultate.Count} programări pentru {numeCautat}:");
                            foreach (var prog in rezultate)
                            {
                                Console.WriteLine($"ID: {prog.Id} | Tip: {prog.TipManichiura}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nu s-a găsit nicio programare.");
                        }
                        break;

                    case "4":
                        ruleaza = false;
                        Console.WriteLine("La revedere!");
                        break;

                    default:
                        Console.WriteLine("Opțiune invalidă!");
                        break;
                }
            }
        }
    }
}