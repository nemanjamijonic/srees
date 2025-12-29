using AutoMapper;
using SREES.Common.Models.Dtos.Regions;
using SREES.DAL.Models;

namespace SREES.BLL.Mappings
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<Region, RegionDataOut>().ReverseMap();
            CreateMap<RegionDataIn, Region>();
            CreateMap<Region, RegionSelectDataOut>();
        }
    }
}
