using ITSM.Dto;

namespace ITSM.Services
{
    public interface IServicesService
    {
        public List<ServiceDto> GetServices();
        public ServiceDto? GetService(int id);
        public ServiceDto CreateService(ServiceDto serviceDto);
        public ServiceDto? UpdateService(ServiceDto serviceDto);
        public void DeleteService(int id);
    }
}
