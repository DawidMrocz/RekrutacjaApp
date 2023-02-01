using AutoMapper;
using MediatR;
using RekrutacjaApp.Dtos;
using RekrutacjaApp.Entities;
using RekrutacjaApp.Repositories;


namespace RekrutacjaApp.Queries
{
    public record GetUserQuery : IRequest<User>
    {
        public required int userId { get; set; }
    }
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, User>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetUserQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Users.GetById(u => u.UserId == request.userId);
        }
    }
}
