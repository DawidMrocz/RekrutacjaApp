using RekrutacjaApp.Entities;

namespace RekrutacjaApp.Dtos
{
    public class UpdateUserDto
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }
        public List<CustomAttribute>? CustomAttributes { get; set; }
    }
}
