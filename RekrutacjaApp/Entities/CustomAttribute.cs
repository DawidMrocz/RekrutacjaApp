
using System.ComponentModel.DataAnnotations;

namespace RekrutacjaApp.Entities
{
        public class CustomAttribute
        {
            public int CustomAttributeId { get; set; }
            [Required]
            [RegularExpression("/[A-Z]{1}[a-z\\s]{1,}")]
            [StringLength(100),MinLength(3)]
            public required string Name { get; set; }
            [Required]
            public required string Value { get; set; }
            public int UserId { get; set; }
            public User? User { get; set; }
    }
}
