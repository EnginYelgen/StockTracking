﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockTracking.Models
{
    public partial class Period
    {
        public Period()
        {
            Estimate = new HashSet<Estimate>();
        }

        public byte Id { get; set; }
        [Display(Name = "Periyot")]
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Estimate> Estimate { get; set; }
    }
}
