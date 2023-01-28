using MediatR;
using RekrutacjaApp.Entities;
using RekrutacjaApp.Repositories;


namespace RekrutacjaApp.Commands
{
    public record CreateUserCommand : IRequest<bool>
    {
        public required User user { get; set; }
    }
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand,bool>
    {
        //private readonly IUserRepository _userRepository;
        //public CreateUserCommandHandler(IUserRepository userRepository)
        //{
        //    _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        //}

        private readonly IUserRepository _userRepository;
        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await _userRepository.CreateUser(request);
            return true;
        }
    }
}
