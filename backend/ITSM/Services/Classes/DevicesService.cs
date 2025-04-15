using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using ITSM.Data;
using ITSM.Dto;
using ITSM.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITSM.Services
{
    public class DevicesService : IDevicesService
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
            return _context.Devices.Select(d => _mapper.Map<DeviceDto>(d)).ToList();
        }

        public DeviceDto? GetDeviceById(int id)
        {
            var device = _context.Devices.Where(d => d.Id == id).FirstOrDefault();
            if (device == null)
                return null;

            return _mapper.Map<DeviceDto>(device);
        }

        public List<DeviceDto> GetDevicesByUser(UserDto user)
        {
            var devices = _context.Devices.Where(d => d.UserId == user.Id);
            return devices.Select(d => _mapper.Map<DeviceDto>(d)).ToList();
        }

        public DeviceDto CreateDevice(DeviceDto deviceDto)
        {
            var device = _mapper.Map<Device>(deviceDto);
            _context.Devices.Add(device);
            _context.SaveChanges();

            return _mapper.Map<DeviceDto>(device);
        }

        public DeviceDto? UpdateDevice(DeviceDto deviceDto)
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
        public byte[] AllDevicesReport()
        {
            var Devices = GetDevices();


            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Devices");

            // Nagłówki
            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "Name";
            worksheet.Cell(1, 3).Value = "Description";
            worksheet.Cell(1, 4).Value = "AcquisitionDate";
            worksheet.Cell(1, 5).Value = "DepreciationDate";
            worksheet.Cell(1, 6).Value = "Status";
            worksheet.Cell(1, 7).Value = "User";

            // Dodanie danych
            for (int i = 0; i < Devices.Count; i++)
            {
                var Device = Devices[i];
                worksheet.Cell(i + 2, 1).Value = Device.Id;
                worksheet.Cell(i + 2, 2).Value = Device.Name;
                worksheet.Cell(i + 2, 3).Value = Device.Description;
                worksheet.Cell(i + 2, 4).Value = Device.AcquisitionDate;
                worksheet.Cell(i + 2, 5).Value = Device.DepreciationDate;
                worksheet.Cell(i + 2, 6).Value = Device.Status;
                worksheet.Cell(i + 2, 7).Value = Device.UserId;
            }


            using var stream = new System.IO.MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public byte[] UserDevicesReport(UserDto user)
        {
            var Devices = GetDevicesByUser(user);


            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("MyDevices");

            // Nagłówki
            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "Name";
            worksheet.Cell(1, 3).Value = "Description";
            worksheet.Cell(1, 4).Value = "AcquisitionDate";
            worksheet.Cell(1, 5).Value = "DepreciationDate";
            worksheet.Cell(1, 6).Value = "Status";

            // Dodanie danych
            for (int i = 0; i < Devices.Count; i++)
            {
                var Device = Devices[i];
                worksheet.Cell(i + 2, 1).Value = Device.Id;
                worksheet.Cell(i + 2, 2).Value = Device.Name;
                worksheet.Cell(i + 2, 3).Value = Device.Description;
                worksheet.Cell(i + 2, 4).Value = Device.AcquisitionDate;
                worksheet.Cell(i + 2, 5).Value = Device.DepreciationDate;
                worksheet.Cell(i + 2, 6).Value = Device.Status;
            }


            using var stream = new System.IO.MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
