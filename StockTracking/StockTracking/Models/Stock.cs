using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockTracking.Models
{
    public partial class Stock
    {
        public Stock()
        {
            Estimate = new HashSet<Estimate>();
        }

        public short Id { get; set; }
        [Display(Name = "Hisse")]
        public string Code { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Estimate> Estimate { get; set; }
    }
}
