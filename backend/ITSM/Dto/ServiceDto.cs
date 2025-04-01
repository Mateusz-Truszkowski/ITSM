namespace ITSM.Dto
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ContractingDate { get; set; }
        public string Status { get; set; }
        public int SLA { get; set; }
    }
}
