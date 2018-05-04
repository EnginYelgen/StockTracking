using System;
using System.Collections.Generic;

namespace StockTracking.Models
{
    public partial class InvesmentCompany
    {
        public InvesmentCompany()
        {
            Estimate = new HashSet<Estimate>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }

        public ICollection<Estimate> Estimate { get; set; }
    }
}
