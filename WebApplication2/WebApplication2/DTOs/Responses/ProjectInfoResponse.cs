using System;

namespace WebApplication2.DTOs.Responses
{
    public class ProjectInfoResponse
    {
        public string TaskTypeName { get; set; }
        public string IdTask { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public string IdTeam { get; set; }
    }
}