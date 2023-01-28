using RekrutacjaApp.Entities;

namespace MyWebApplication.Dtos
{
    public class QueryParams
    {
        public string? SearchString { get; set; }
        public string? SortOrder { get; set; }
        public Gender? Gender { get; set; }
        public bool? CarLicense { get; set; }
    }
}