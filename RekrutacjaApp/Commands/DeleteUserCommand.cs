using MediatR;
using RekrutacjaApp.Entities;
using RekrutacjaApp.Repository;

namespace RekrutacjaApp.Commands
{
    public record DeleteUserCommand : IRequest<Unit>
    {
        public required int UserId { get; set; }
    }
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.Users.Delete(request.UserId);
            return Task.FromResult(Unit.Value);
        }
    }
}
