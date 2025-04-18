﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITSM.Entity
{
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? SolutionDate { get; set; }
        public string? SolutionDescription { get; set; }
        public int Priority { get; set; }
        public string Type { get; set; }
        public string Status {  get; set; }
        public int ServiceId { get; set; }
        [ValidateNever]
        public Service? Service { get; set; }
        public int RequesterId { get; set; }
        [ValidateNever]
        public User? Requester { get; set; }
        public int AssigneeId { get; set; }
        [ValidateNever]
        public User? Assignee { get; set; }
    }
}
