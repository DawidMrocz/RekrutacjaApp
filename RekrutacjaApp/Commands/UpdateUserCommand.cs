using MediatR;
using RekrutacjaApp.Dtos;
using RekrutacjaApp.Entities;
using RekrutacjaApp.Repositories;


namespace RekrutacjaApp.Commands
{
    public record UpdateUserCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public User user { get; set; }
    }
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand,bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Users.Update(request.user);
            await _unitOfWork.Save();
            return true;
        }
    }
}
