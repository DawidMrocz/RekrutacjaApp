using RekrutacjaApp.Entities;
using System.ComponentModel.DataAnnotations;

namespace RekrutacjaApp.Dtos
{
    public class CustomAttributeDto
    {
        public int CustomAttributeId { get; set; }
        public required string Name { get; set; }
        public required string Value { get; set; }
    }
}
