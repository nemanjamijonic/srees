using AutoMapper;
using SREES.Common.Models.Dtos.Substations;
using SREES.DAL.Models;

namespace SREES.BLL.Mappings
{
    public class SubstationProfile : Profile
    {
        public SubstationProfile()
        {
            CreateMap<Substation, SubstationDataOut>().ReverseMap();
            CreateMap<SubstationDataIn, Substation>();
            CreateMap<Substation, SubstationSelectDataOut>();
        }
    }
}
