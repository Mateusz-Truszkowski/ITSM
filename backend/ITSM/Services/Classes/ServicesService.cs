using AutoMapper;
using ITSM.Data;
using ITSM.Dto;
using ITSM.Entity;

namespace ITSM.Services
{
    public class ServicesService : IServicesService
    {
        private readonly IMapper _mapper;
        private readonly ITSMContext _context;

        public ServicesService(ITSMContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<ServiceDto> GetServices()
        {
            return _context.Services.Select(service => _mapper.Map<ServiceDto>(service)).ToList();
        }

        public ServiceDto? GetService(int id)
        {
            var service = _context.Services.Where(s => s.Id == id).FirstOrDefault();
            if (service == null)
                return null;

            return _mapper.Map<ServiceDto>(service);
        }

        public ServiceDto CreateService(ServiceDto serviceDto)
        {
            Service service = _mapper.Map<Service>(serviceDto);
            _context.Services.Add(service);
            _context.SaveChanges();
            return _mapper.Map<ServiceDto>(service);
        }

        public ServiceDto? UpdateService(ServiceDto serviceDto)
        {
            var service = _context.Services.Where(s => s.Id == serviceDto.Id).FirstOrDefault();
            if (service == null)
                return null;

            if (!string.IsNullOrEmpty(serviceDto.Name))
                service.Name = serviceDto.Name;

            if (!string.IsNullOrEmpty(serviceDto.Description))
                service.Description = serviceDto.Description;

            if (serviceDto.ContractingDate != DateTime.MinValue)
                service.ContractingDate = serviceDto.ContractingDate;

            if (serviceDto.SLA != 0)
                service.SLA = serviceDto.SLA;

            if (!string.IsNullOrEmpty(serviceDto.Status))
                service.Status = serviceDto.Status;

            _context.SaveChanges();

            return _mapper.Map<ServiceDto>(service);
        }

        public void DeleteService(int id)
        {
            var service = _context.Services.Where(s => s.Id == id).FirstOrDefault();
            if (service == null) 
                return;

            _context.Services.Remove(service);
            _context.SaveChanges();
        }
    }
}
