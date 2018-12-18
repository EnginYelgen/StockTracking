using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StockTracking.Common
{
    public class Attributes
    {
        public class MinValueAttribute : ValidationAttribute
        {
            int _val;

            public MinValueAttribute(int val)
            {
                _val = val;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                ViewModels.CreateBulkyModel model = (ViewModels.CreateBulkyModel)validationContext.ObjectInstance;

                string mess = string.Empty;

                if (Convert.ToInt32(value) < _val)
                {
                    mess = $"En küçük değer {_val} olmalı.";
                    return new ValidationResult(mess);
                }

                return ValidationResult.Success;
            }
        }

        public class MaxValueAttribute : ValidationAttribute
        {
            int _val;

            public MaxValueAttribute(int val)
            {
                _val = val;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                ViewModels.CreateBulkyModel model = (ViewModels.CreateBulkyModel)validationContext.ObjectInstance;

                string mess = string.Empty;

                if (Convert.ToInt32(value) > _val)
                {
                    mess = $"En büyük değer {_val} olmalı.";
                    return new ValidationResult(mess);
                }

                return ValidationResult.Success;
            }
        }

        public class EstimateStartDateAttribute : ValidationAttribute
        {
            public EstimateStartDateAttribute() { }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                ViewModels.CreateBulkyModel model = (ViewModels.CreateBulkyModel)validationContext.ObjectInstance;

                string mess = string.Empty;

                if (model.PeriodId == 1 && model.StartDate != model.EndDate)
                {
                    mess = "Başlangış ve bitiş tarihi eşit olmalı.";
                    return new ValidationResult(mess);
                }

                return ValidationResult.Success;
            }
        }

        public class EstimateEndDateAttribute : ValidationAttribute
        {
            public EstimateEndDateAttribute() { }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                ViewModels.CreateBulkyModel model = (ViewModels.CreateBulkyModel)validationContext.ObjectInstance;

                string mess = string.Empty;

                if (model.PeriodId == 1 && model.StartDate != model.EndDate)
                {
                    mess = "Başlangış ve bitiş tarihi eşit olmalı.";
                    return new ValidationResult(mess);
                }

                return ValidationResult.Success;
            }
        }

        public class TargetPriceAttribute : ValidationAttribute
        {
            public TargetPriceAttribute() { }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                ViewModels.CreateBulkyModel model = (ViewModels.CreateBulkyModel)validationContext.ObjectInstance;

                string mess = string.Empty;

                if (model.TargetPrice <= model.OpeningPrice)
                {
                    mess = "Hedef fiyat başlangıç fiyatından büyük olmalı.";
                    return new ValidationResult(mess);
                }

                return ValidationResult.Success;
            }
        }
    }
}
