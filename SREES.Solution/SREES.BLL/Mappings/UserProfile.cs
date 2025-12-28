using AutoMapper;
using SREES.Common.Models.Dtos.Users;
using SREES.DAL.Models;

namespace SREES.BLL.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDataOut>().ReverseMap();
            CreateMap<UserDataIn, User>();
        }
    }
}
