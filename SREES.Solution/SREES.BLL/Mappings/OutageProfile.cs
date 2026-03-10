using AutoMapper;
using SREES.Common.Models.Dtos.Outages;
using SREES.DAL.Models;

namespace SREES.BLL.Mappings
{
    public class OutageProfile : Profile
    {
        public OutageProfile()
        {
            CreateMap<Outage, OutageDataOut>()
                .ForMember(dest => dest.RegionName, opt => opt.MapFrom(src => src.Region != null ? src.Region.Name : null))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? $"{src.Customer.FirstName} {src.Customer.LastName}" : null))
                .ForMember(dest => dest.BuildingAddress, opt => opt.MapFrom(src => src.Building != null ? src.Building.Address : null))
                .ForMember(dest => dest.DetectedSubstationName, opt => opt.MapFrom(src => src.DetectedSubstation != null ? src.DetectedSubstation.Name : null))
                .ForMember(dest => dest.DetectedFeederName, opt => opt.MapFrom(src => src.DetectedFeeder != null ? src.DetectedFeeder.Name : null))
                .ForMember(dest => dest.DetectedPoleName, opt => opt.MapFrom(src => src.DetectedPole != null ? src.DetectedPole.Name : null));
            
            CreateMap<OutageDataIn, Outage>();
        }
    }
}
