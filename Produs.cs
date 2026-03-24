using System;

namespace SalonUnghii.Models
{
    public class Produs
    {
        public int Id { get; set; }
        public string Nume { get; set; } = "";
        public int Cantitate { get; set; }
        public decimal Pret { get; set; }
    }
}