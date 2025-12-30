using AutoMapper;
using SREES.Common.Models.Dtos.Customers;
using SREES.DAL.Models;

namespace SREES.BLL.Mappings
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDataOut>().ReverseMap();
            CreateMap<CustomerDataIn, Customer>();
            CreateMap<Customer, CustomerSelectDataOut>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        }
    }
}
