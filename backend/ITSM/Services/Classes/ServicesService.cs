using AutoMapper;
using ClosedXML.Excel;
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
        public byte[] AllServicesReport()
        {
            var services = GetServices();


            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Users");

            // Nagłówki
            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "Name";
            worksheet.Cell(1, 3).Value = "Description";
            worksheet.Cell(1, 4).Value = "ContractingDate";
            worksheet.Cell(1, 5).Value = "Status";
            worksheet.Cell(1, 6).Value = "SLA";

            // Dodanie danych
            for (int i = 0; i < services.Count; i++)
            {
                var service = services[i];
                worksheet.Cell(i + 2, 1).Value = service.Id;
                worksheet.Cell(i + 2, 2).Value = service.Name;
                worksheet.Cell(i + 2, 3).Value = service.Description;
                worksheet.Cell(i + 2, 4).Value = service.ContractingDate;
                worksheet.Cell(i + 2, 5).Value = service.Status;
                worksheet.Cell(i + 2, 6).Value = service.SLA;
            }


            using var stream = new System.IO.MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
