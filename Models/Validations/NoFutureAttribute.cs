using System;
using System.ComponentModel.DataAnnotations;

namespace LibreriaWeb.Models.Validations
{
    public class NoFutureAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int year)
            {
                if (year > DateTime.Now.Year)
                {
                    return new ValidationResult(ErrorMessage ?? "El año de publicación no puede ser mayor al año actual.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
