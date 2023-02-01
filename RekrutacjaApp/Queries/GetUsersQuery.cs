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
            return await _unitOfWork.Users.GetAll();
        }
    }
}
