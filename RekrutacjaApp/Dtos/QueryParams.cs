namespace MyWebApplication.Dtos
{
    public class QueryParams
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 10;
    }
}