using AutoMapper;
using SREES.Common.Models.Dtos.Outages;
using SREES.DAL.Models;

namespace SREES.BLL.Mappings
{
    public class OutageProfile : Profile
    {
        public OutageProfile()
        {
            CreateMap<Outage, OutageDataOut>().ReverseMap();
            CreateMap<OutageDataIn, Outage>();
        }
    }
}
