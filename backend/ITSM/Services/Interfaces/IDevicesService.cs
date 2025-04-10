using ITSM.Dto;

namespace ITSM.Services
{
    public interface IDevicesService
    {
        public List<DeviceDto> GetDevices();
        public DeviceDto? GetDeviceById(int id);
        public List<DeviceDto> GetDevicesByUser(UserDto user);
        public DeviceDto CreateDevice(DeviceDto deviceDto);
        public DeviceDto? UpdateDevice(DeviceDto deviceDto);
        public void DeleteDevice(int id);
    }
}
