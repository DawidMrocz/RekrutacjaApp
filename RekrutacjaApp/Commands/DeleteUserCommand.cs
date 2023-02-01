using MediatR;
using RekrutacjaApp.Entities;
using RekrutacjaApp.Repositories;


namespace RekrutacjaApp.Commands
{
    public record DeleteUserCommand : IRequest<bool>
    {
        public required int UserId { get; set; }
    }
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Users.Delete(request.UserId);
            return true;
        }
    }
}
