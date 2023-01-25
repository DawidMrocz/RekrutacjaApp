

using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RekrutacjaApp.Helpers
{
    public class CustomPersonalValidator : ValidationAttribute
    {
        public CustomPersonalValidator(int lenghtOfWord)
            => LenghtOfWord = lenghtOfWord;

        public int LenghtOfWord { get; }
        string pattern = @"/[A-Z]{1}+[a-z]{1,}/g";

        public string GetErrorMessage() =>
            $"Fraza musi mieć przynajmniej 2 znaki i byc dłuższa niż {LenghtOfWord} znaków.";

        public string GetErrorMessage2() =>
            $"Imię powinno składać się z samych liter oraz zaczynać się od dużej litery.";

        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            //var user = (User)validationContext.ObjectInstance;
            string inputValue = (string)value!;

            if (inputValue.Length > LenghtOfWord || inputValue.Length < 2)
            {
                return new ValidationResult(GetErrorMessage());
            }

            Match m = Regex.Match(inputValue, pattern, RegexOptions.IgnoreCase);

            if (!m.Success) return new ValidationResult(GetErrorMessage2());

            return ValidationResult.Success;
        }
    }
}
