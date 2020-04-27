using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.DTOs.Requests
{
    public class AddTaskRequest
    {
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        [Required] public DateTime Deadline { get; set; }
        [Required] public string IdProject { get; set; }
        [Required] public string IdAssignedTo { get; set; }
        [Required] public string IdCreator { get; set; }
    }
}