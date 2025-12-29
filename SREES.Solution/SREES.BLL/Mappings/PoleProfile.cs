using AutoMapper;
using SREES.Common.Models.Dtos.Poles;
using SREES.DAL.Models;

namespace SREES.BLL.Mappings
{
    public class PoleProfile : Profile
    {
        public PoleProfile()
        {
            CreateMap<Pole, PoleDataOut>().ReverseMap();
            CreateMap<PoleDataIn, Pole>();
            CreateMap<Pole, PoleSelectDataOut>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address ?? $"Pole {src.Id}"));
        }
    }
}
