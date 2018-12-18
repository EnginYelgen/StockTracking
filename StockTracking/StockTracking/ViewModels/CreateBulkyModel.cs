using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using StockTracking.Common;

namespace StockTracking.ViewModels
{
    public class CreateBulkyModel
    {
        public Guid Key { get; set; }
        [Display(Name = "Firma")]
        public byte InvesmentCompanyId { get; set; }
        [Display(Name = "Hisse")]
        public short StockId { get; set; }
        [Display(Name = "Periyot")]
        public byte PeriodId { get; set; }
        [Attributes.EstimateStartDate]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Display(Name = "Başlangıç Tarihi")]
        public DateTime StartDate { get; set; }
        [Attributes.EstimateEndDate]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Display(Name = "Bitiş Tarihi")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Başlangıç Fiyatı")]
        public decimal OpeningPrice { get; set; }
        [Attributes.TargetPrice]
        [Display(Name = "Hedef Fiyatı")]
        public decimal TargetPrice { get; set; }
        [Display(Name = "Kapanış Fiyatı")]
        public decimal? ClosingPrice { get; set; }

        [Display(Name = "Firma")]
        public string InvesmentCompanyName { get; set; }
        [Display(Name = "Hisse")]
        public string StockCode { get; set; }
        [Display(Name = "Periyot")]
        public string PeriodName { get; set; }

        public ICollection<CreateBulkyModel> Estimates { get; set; }
    }
}
