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

            CreateMap<Device, DeviceDto>()
                .ForMember(dest => dest.User, opt => opt.Ignore());
            CreateMap<DeviceDto, Device>();

            CreateMap<Ticket, TicketDto>()
                .ForMember(dest => dest.Requester, opt => opt.Ignore())
                .ForMember(dest => dest.Assignee, opt => opt.Ignore());
            CreateMap<TicketDto, Ticket>();
        }
    }
}
