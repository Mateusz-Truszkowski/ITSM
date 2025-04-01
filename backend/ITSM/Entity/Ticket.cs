using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace backend.Entity
{
    public class Ticket
    {
        int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }

        public int ServiceId { get; set; }
        [ValidateNever]
        public Service Service { get; set; }

        public int AssigneeId { get; set; }
        [ValidateNever]
        public User Assignee { get; set; }
    }
}
