using MediatR;
using RekrutacjaApp.Repositories;

namespace RekrutacjaApp.Commands
{
    public record RemoveAttributeCommand : IRequest<bool>
    {
        public required int Id { get; set; }
    }
    public class RemoveAttributeCommandHandler : IRequestHandler<RemoveAttributeCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        public RemoveAttributeCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }
        public async Task<bool> Handle(RemoveAttributeCommand request, CancellationToken cancellationToken)
        {
            await _userRepository.RemoveAttribute(request);
            return true;
        }
    }
}
