using RekrutacjaApp.Entities;

namespace MyWebApplication.Dtos
{
    public class QueryParams
    {
        public string? SearchString { get; set; }
        public int? AgeMax { get; set; }
        public int? AgeMin { get; set; }
        public string? SortOrder { get; set; }
        public Gender? Gender { get; set; }
        //public int page { get; set; } = 1;
        //public int pageSize { get; set; } = 10;
    }
}