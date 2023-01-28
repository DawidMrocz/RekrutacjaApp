using RekrutacjaApp.Entities;

namespace RekrutacjaApp.Dtos
{
    public class UserDto
    {
        public int UserId { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required DateTime BirthDate { get; set; }
        public required Gender Gender { get; set; }
        public required bool CarLicense { get; set; }
        public List<CustomAttributeDto>? CustomAttributes { get; set; }
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
    }
}
