using System;

namespace SalonUnghii.Models
{
    public class Manichiura
    {
        public int Id { get; set; }
        public string Tip { get; set; } = "";
        public decimal Pret { get; set; }
        public int DurataMinute { get; set; }
    }
}