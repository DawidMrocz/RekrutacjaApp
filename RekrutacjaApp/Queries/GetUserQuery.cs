using AutoMapper;
using MediatR;
using RekrutacjaApp.Dtos;
using RekrutacjaApp.Repository;

namespace RekrutacjaApp.Queries
{
    public record GetUserQuery : IRequest<UserDto>
    {
        public required int userId { get; set; }
    }
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<UserDto>(await _unitOfWork.Users.GetById(request.userId));
        }
    }
}
