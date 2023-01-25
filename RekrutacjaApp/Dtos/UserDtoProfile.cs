using AutoMapper;
using RekrutacjaApp.Entities;

namespace RekrutacjaApp.Dtos
{
    public class UserDtoProfile : Profile
    {
        public UserDtoProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
