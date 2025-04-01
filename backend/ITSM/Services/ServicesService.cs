using ITSM.Data;
using ITSM.Dto;
using ITSM.Entity;

namespace ITSM.Services
{
    public class ServicesService
    {
        private ITSMContext _context;

        public ServicesService(ITSMContext context)
        {
            _context = context;
        }

        public List<ServiceDto> GetServices()
        {
            return _context.Services.Select(
                service => 
                new ServiceDto 
                {   
                    Id = service.Id, 
                    Name =  service.Name, 
                    Description = service.Description, 
                    ContractingDate = service.ContractingDate, 
                    SLA = service.SLA, 
                    Status = service.Status 
                }).ToList();
        }

        public ServiceDto GetService(int id)
        {
            var service = _context.Services.Where(s => s.Id == id).FirstOrDefault();
            return new ServiceDto { Id = service.Id, Name = service.Name, Description = service.Description, ContractingDate = service.ContractingDate, SLA = service.SLA, Status = service.Status };
        }

        public ServiceDto CreateService(ServiceDto serviceDto)
        {
            Service service = new Service { Id = serviceDto.Id, Name = serviceDto.Name, Description = serviceDto.Description, ContractingDate = serviceDto.ContractingDate, SLA = serviceDto.SLA, Status = serviceDto.Status };
            _context.Services.Add(service);
            _context.SaveChanges();
            return new ServiceDto { Id = service.Id, Name = service.Name, Description = service.Description, ContractingDate= service.ContractingDate, SLA = service.SLA, Status = service.Status };
        }

        public ServiceDto UpdateService(ServiceDto serviceDto)
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

            return new ServiceDto { Id = service.Id, Name = service.Name, Description = service.Description,ContractingDate = service.ContractingDate, SLA = service.SLA, Status = service.Status };
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
