using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRBackend.Model
{
    public class TransactionExcelModel
    {
        public string MonthYear { get; set; }
        public string Type { get; set; }
        public string Version { get; set; }
        public string Unit { get; set; }
        public string EntryType { get; set; }
        public string Header { get; set; }
        public string SubHeader { get; set; }
        public string ItemName { get; set; }
        public decimal? Value { get; set; }
        public int? NumberOfUpdate { get; set; }
    }
}
