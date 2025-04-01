using ITSM.Entity;

namespace ITSM.Dto
{
    public class DeviceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public DateTime DepreciationDate { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string Status { get; set; }
    }
}
