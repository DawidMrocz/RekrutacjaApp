using RekrutacjaApp.Entities;

namespace MyWebApplication.Dtos
{
    public class PagedResult<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<User>? Results { get; set; }
    }
}