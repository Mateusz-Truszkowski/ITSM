using AutoMapper;
using ITSM.Data;
using ITSM.Dto;
using ITSM.Entity;
using Microsoft.EntityFrameworkCore;

namespace ITSM.Services
{
    public class DevicesService
    {
        private readonly ITSMContext _context;
        private readonly IMapper _mapper;

        public DevicesService(ITSMContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<DeviceDto> GetDevices()
        {
            return _context.Devices.Include(d => d.User).Select(d => _mapper.Map<DeviceDto>(d)).ToList();
        }

        public DeviceDto GetDeviceById(int id)
        {
            var device = _context.Devices.Where(d => d.Id == id).FirstOrDefault();
            if (device == null)
                return null;

            return _mapper.Map<DeviceDto>(device);
        }

        public DeviceDto CreateDevice(DeviceDto deviceDto)
        {
            var device = _mapper.Map<Device>(deviceDto);
            _context.Devices.Add(device);
            _context.SaveChanges();

            return _mapper.Map<DeviceDto>(device);
        }

        public DeviceDto UpdateDevice(DeviceDto deviceDto)
        {
            var device = _context.Devices.Where(d => d.Id == deviceDto.Id).FirstOrDefault();

            if (device == null)
                return null;

            if (!string.IsNullOrEmpty(deviceDto.Name))
                device.Name = deviceDto.Name;

            if (!string.IsNullOrEmpty(deviceDto.Description))
                device.Description = deviceDto.Description;

            if (deviceDto.AcquisitionDate != DateTime.MinValue)
                device.AcquisitionDate = deviceDto.AcquisitionDate;

            if (deviceDto.DepreciationDate != DateTime.MinValue)
                device.DepreciationDate = deviceDto.DepreciationDate;

            if (deviceDto.UserId != 0)
                device.UserId = deviceDto.UserId;

            if (!string.IsNullOrEmpty(deviceDto.Status))
                device.Status = deviceDto.Status;

            _context.SaveChanges();

            return _mapper.Map<DeviceDto>(device);
        }

        public void DeleteDevice(int id)
        {
            var device = _context.Devices.Where(d => d.Id == id).FirstOrDefault();

            if (device == null)
                return;

            _context.Devices.Remove(device);
            _context.SaveChanges();
        }
    }
}
