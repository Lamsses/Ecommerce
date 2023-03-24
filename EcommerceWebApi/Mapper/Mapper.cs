using AutoMapper;
using EcommerceLibrary.Models;

namespace EcommerceWebApi.Mapper
{
    public class Mapper : Profile
    {
       
    
        public Mapper()
        {
           
            CreateMap<AuthenticationModel, CustomersModel>();

        }
    }
}
