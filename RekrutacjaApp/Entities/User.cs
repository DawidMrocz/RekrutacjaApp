using Microsoft.AspNetCore.Mvc;
using RekrutacjaApp.Helpers;
using System.ComponentModel.DataAnnotations;

namespace RekrutacjaApp.Entities
{
    public enum Gender
    {
        Mężczyzna,
        Kobieta,
    }



    public class User : IValidatableObject
    {

        public DateTime currentDate = DateTime.Now;

        public int UserId { get; set; }
        [Required]
        [CustomPersonalValidator(50)]
        public required string Name { get; set; }
        [Required]
        [CustomPersonalValidator(150)]
        public required string Surname { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public required DateTime BirthDate { get; set; }
        [Required]
        [EnumDataType(typeof(Gender))]
        public required Gender Gender { get; set; }
        public required bool CarLicense { get; set; }
        public List<CustomAttribute>? CustomAttributes { get; set; } = new();    
        public string DisplayName
        {
            get
            {
                return $"{Name} {Surname}";
            }
        }
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - BirthDate.Year;
                if (BirthDate > today.AddYears(-age)) age--;
                return age;
            }
        }
        public string Title
        {
            get
            {
                return Gender == Gender.Mężczyzna ? "Pan" : "Pani";
            }
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BirthDate > currentDate)
            {
                yield return new ValidationResult(
                    $"Twoja data urodzenia nie może być większa niż {currentDate}.",
                    new[] { nameof(BirthDate) });
            }
        }
    }
}
