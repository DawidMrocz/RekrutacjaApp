using MediatR;
using RekrutacjaApp.Dtos;
using RekrutacjaApp.Entities;
using RekrutacjaApp.Repository;

namespace RekrutacjaApp.Commands
{
    public record UpdateUserCommand : IRequest<Unit>
    {
        public int UserId { get; set; }
        public required User user { get; set; }
    }
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.Users.Update(request.user,request.UserId);
            return Task.FromResult(Unit.Value);
        }
    }
}
