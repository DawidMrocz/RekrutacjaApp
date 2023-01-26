using MediatR;
using RekrutacjaApp.Entities;
using RekrutacjaApp.Repository;

namespace RekrutacjaApp.Commands
{
    public record CreateUserCommand : IRequest<Unit>
    {
        public required User user { get; set; }
    }
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Unit>
    {    
        private readonly IUnitOfWork _unitOfWork;
        public CreateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.UserRepository.Create(request.user);
            return Task.FromResult(Unit.Value);
        }
    }
}
