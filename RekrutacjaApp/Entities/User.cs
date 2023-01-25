using Microsoft.AspNetCore.Mvc;
using RekrutacjaApp.Helpers;
using System.ComponentModel.DataAnnotations;

namespace RekrutacjaApp.Entities
{
    public enum Gender
    {
        Male,
        Famale,
        Other
    }
    public class User : IValidatableObject
    {

        private int currentYear = DateTime.Now.Year;

        public int UserId { get; set; }
        [Required]
        [CustomPersonalValidator(50)]
        [Remote(action: "VerifyName", controller: "Users", AdditionalFields = nameof(Name))]
        public required string Name { get; set; }
        [Required]
        [CustomPersonalValidator(150)]
        [Remote(action: "VerifyName", controller: "Users", AdditionalFields = nameof(Surname))]
        public required string Surname { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public required DateTime BirthDate { get; set; }
        [Required]
        [EnumDataType(typeof(Gender))]
        public required Gender Gender { get; set; }
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
                return Gender == Gender.Male ? "Pan" : "Pani";
            }
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BirthDate.Year > currentYear)
            {
                yield return new ValidationResult(
                    $"Classic movies must have a release year no later than {currentYear}.",
                    new[] { nameof(BirthDate) });
            }
        }

    }
}
