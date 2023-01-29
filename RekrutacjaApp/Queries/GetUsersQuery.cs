using AutoMapper;
using MediatR;
using MyWebApplication.Dtos;
using RekrutacjaApp.Dtos;
using RekrutacjaApp.Entities;
using RekrutacjaApp.Repositories;


namespace RekrutacjaApp.Queries
{
    public record GetUsersQuery : IRequest<List<UserDto>>
    {
        public QueryParams? queryParams { get; set; }
    }
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
    {
        //private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepostiory;
        //public GetUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        //{
        //    _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        //    _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        //}

        public GetUsersQueryHandler(IUserRepository userRepostiory)
        {
            _userRepostiory = userRepostiory ?? throw new ArgumentNullException(nameof(userRepostiory));
        }


        public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            //return _mapper.Map<List<UserDto>>(await _unitOfWork.Users.GetAll(request.queryParams));
            return await _userRepostiory.GetUsers(request);
        }

    }
}
