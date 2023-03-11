using AutoMapper;
using EcommerceLibrary.Models;
using static TodoApi.Controllers.AuthenticationController;

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
