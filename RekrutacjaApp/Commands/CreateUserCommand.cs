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
        private readonly IUnitOfWork _unitOfWork;
        public CreateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Users.Add(request.user);
            return true;
        }
    }
}
