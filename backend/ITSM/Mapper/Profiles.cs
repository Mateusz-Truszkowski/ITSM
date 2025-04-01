using AutoMapper;
using ITSM.Dto;
using ITSM.Entity;

namespace ITSM.Mapper
{
    public class Profiles : Profile
    {
        public Profiles() 
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Service, ServiceDto>();
            CreateMap<ServiceDto, Service>();
        }
    }
}
