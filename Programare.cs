using System;

namespace SalonUnghii.Models
{
    public class Programare
    {
        public int Id { get; set; }
        public string NumeClienta { get; set; } = "";
        public DateTime Data { get; set; }
        public string TipManichiura { get; set; } = "";
    }
}