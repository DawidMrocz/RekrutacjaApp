using AutoMapper;
using MediatR;
using MyWebApplication.Dtos;
using RekrutacjaApp.Dtos;
using RekrutacjaApp.Entities;
using RekrutacjaApp.Repositories;


namespace RekrutacjaApp.Queries
{
    public record GetUsersQuery : IRequest<List<User>>
    {
        public QueryParams? queryParams { get; set; }
    }
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<User>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetUsersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<List<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            List<string> properties = new List<string>();
            properties.Add("CustomAttributes");
            return await _unitOfWork.Users.GetAll(u => u.Name.Contains(request.queryParams.SearchString), orderedBy: q => q.OrderBy(d => request.queryParams.SortOrder == "name" ? d.Name : d.Surname),includes: properties);
        }
    }
}
