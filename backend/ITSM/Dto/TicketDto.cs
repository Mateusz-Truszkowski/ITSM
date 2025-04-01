using ITSM.Entity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ITSM.Dto
{
    public class TicketDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? SolutionDate { get; set; }
        public string? SolutionDescription { get; set; }
        public int Priority { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public int ServiceId { get; set; }
        public Service? Service { get; set; }
        public int RequesterId { get; set; }
        public User? Requester { get; set; }
        public int AssigneeId { get; set; }
        public User? Assignee { get; set; }
    }
}
