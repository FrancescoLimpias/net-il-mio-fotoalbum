using System.ComponentModel.DataAnnotations;

namespace il_mio_fotoalbum.Utilities
{
    public class LongDescriptionValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            //Description is null
            if (value == null)
            {
                return ValidationResult.Success;
            }

            string description = (string)value;

            // Description is not long enough
            if (description.Split(" ").Count() < 3)
                return new ValidationResult("La descrizione, se inserita, deve avere almeno 3 parole");

            return ValidationResult.Success;
        }
    }
}
