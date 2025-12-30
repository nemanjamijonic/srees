using AutoMapper;
using SREES.Common.Models.Dtos.Feeders;
using SREES.DAL.Models;

namespace SREES.BLL.Mappings
{
    public class FeederProfile : Profile
    {
        public FeederProfile()
        {
            CreateMap<Feeder, FeederDataOut>().ReverseMap();
            CreateMap<FeederDataIn, Feeder>();
            CreateMap<Feeder, FeederSelectDataOut>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name ?? $"Feeder {src.Id}"));
        }
    }
}
