﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockTracking.Models
{
    public partial class Estimate
    {
        public int Id { get; set; }
        [Display(Name = "Firma")]
        public byte InvesmentCompanyId { get; set; }
        [Display(Name = "Hisse")]
        public short StockId { get; set; }
        [Display(Name = "Periyot")]
        public byte PeriodId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Display(Name = "Başlangıç Tarihi")]
        public DateTime StartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Display(Name = "Bitiş Tarihi")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Başlangıç Fiyatı")]
        public decimal OpeningPrice { get; set; }
        [Display(Name = "Hedef Fiyatı")]
        public decimal? TargetPrice { get; set; }
        [Display(Name = "Kapanış Fiyatı")]
        public decimal? ClosingPrice { get; set; }

        public InvesmentCompany InvesmentCompany { get; set; }
        public Stock Stock { get; set; }
        public Period Period { get; set; }
    }
}
