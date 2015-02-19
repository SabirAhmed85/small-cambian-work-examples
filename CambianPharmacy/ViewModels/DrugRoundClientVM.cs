using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CambianPharmacy.ViewModels
{
    public class DrugRoundClientVM
    {
        public string FullName { get; set; }
        public Int64 CambianClientID { get; set; }
        public int Doses { get; set; }
        public int CompletedDoses { get; set; }
        public int DosesTodo { get; set; }
        public bool ClientComplete { get; set; }
        public bool ClientIncomplete { get; set; }
    }
}