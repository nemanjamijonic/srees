using AutoMapper;
using SREES.Common.Models.Dtos.Buildings;
using SREES.DAL.Models;

namespace SREES.BLL.Mappings
{
    public class BuildingProfile : Profile
    {
        public BuildingProfile()
        {
            CreateMap<Building, BuildingDataOut>()
                .ForMember(dest => dest.PoleType, opt => opt.MapFrom(src => src.Pole != null ? src.Pole.PoleType : (SREES.Common.Constants.PoleType?)null))
                .ReverseMap();
            
            CreateMap<BuildingDataIn, Building>();
            
            CreateMap<Building, BuildingSelectDataOut>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address ?? $"Building {src.Id}"));
        }
    }
}
