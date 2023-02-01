using MediatR;
using RekrutacjaApp.Dtos;
using RekrutacjaApp.Repositories;

namespace RekrutacjaApp.Commands
{
    public record AddAttributeCommand : IRequest<Unit>
    {
        public required int Id { get; set; }
        public required CustomAttributeDto attribute { get; set; }
    }
    public class AddAttributeCommandHandler : IRequestHandler<AddAttributeCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        public AddAttributeCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<Unit> Handle(AddAttributeCommand request, CancellationToken cancellationToken)
        {
            await _userRepository.AddAttribute(request.attribute,request.Id);
            return Unit.Value;
        }
    }
}
