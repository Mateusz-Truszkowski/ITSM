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
            CreateMap<CreateUserDto, User>();

            CreateMap<Service, ServiceDto>();
            CreateMap<ServiceDto, Service>();

            CreateMap<Device, DeviceDto>();
            CreateMap<DeviceDto, Device>();

            CreateMap<Ticket, TicketDto>().ForMember(dest => dest.Requester, opt => opt.Ignore());
            CreateMap<TicketDto, Ticket>();
        }
    }
}
