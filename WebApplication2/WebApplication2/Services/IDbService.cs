using System.Collections.Generic;
using WebApplication2.DTOs.Requests;
using WebApplication2.DTOs.Responses;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public interface IDbService
    {
        public List<ProjectInfoResponse> GetInfoAboutProject(string projectId);
        void addTaskRequestAndTaskTypeIfNotExists(AddTaskRequest taskRequest, Task taskType);
    }
}