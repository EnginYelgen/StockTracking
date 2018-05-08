using System;
using System.Collections.Generic;

namespace StockTracking.Models
{
    public partial class Estimate
    {
        public int Id { get; set; }
        public byte InvesmentCompanyId { get; set; }
        public short StockId { get; set; }
        public byte PeriodId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal OpeningPrice { get; set; }
        public decimal? TargetPrice { get; set; }
        public decimal? ClosingPrice { get; set; }

        public InvesmentCompany InvesmentCompany { get; set; }
        public Stock Stock { get; set; }
        public Period Period { get; set; }
    }
}
