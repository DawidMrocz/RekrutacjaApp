using MediatR;
using MyWebApplication.Dtos;
using RekrutacjaApp.Entities;
using RekrutacjaApp.Repositories;
using System.Linq.Expressions;

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
            List<Expression<Func<User, bool>>> filters = new List<Expression<Func<User, bool>>>();
            filters.Add(u => u.Name.Contains(request.queryParams.SearchString));
            if(request.queryParams.CarLicense is not null) filters.Add(u => u.CarLicense == request.queryParams.CarLicense);
            if (request.queryParams.Gender is not null) filters.Add(u => u.Gender == request.queryParams.Gender);
            List<string> properties = new List<string>();
            properties.Add("CustomAttributes");
            return await _unitOfWork.Users.GetAll(
                filters,
                orderedBy: q => q.OrderBy(d => request.queryParams.SortOrder == "name" ? d.Name : d.Surname),
                includes: properties
                );
        }
    }
}
