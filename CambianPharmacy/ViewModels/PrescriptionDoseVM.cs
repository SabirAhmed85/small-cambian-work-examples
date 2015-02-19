using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ActiveCareForms.Models;

namespace CambianPharmacy.ViewModels
{
    public class PrescriptionDoseVM
    {
        public Int64 DoseID { get; set; }
        public int PrescriptionID { get; set; }
        public int Hour { get; set; }
        public string PrescriptionTypeName { get; set; }
        public string VTMName { get; set; }
        public int ResultCode { get; set; }
    }

    public class ClientDueTodayVM
    {
        public Client Client { get; set; }
        public List<PrescriptionDoseVM> Doses { get; set; }
    }

    public class DrugRoundVM
    {
        public int Hour { get; set; }
        public int ClientsComplete { get; set; }
        public int ClientsIncomplete { get; set; }
        public bool RoundComplete { get; set; }
        public bool RoundIncomplete { get; set; }
        public int Clients { get; set; }
    }
}