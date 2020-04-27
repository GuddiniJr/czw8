using System;
using System.Data.SqlClient;
using WebApplication2.DTOs.Requests;
using WebApplication2.Models;
using WebApplication2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;

namespace WebApplication2.Controllers
{    [ApiController]
    [Route("/api/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly IDbService _service;

        public TaskController(IDbService dbService)
        {
            this._service = dbService;
        }

        [HttpPost]
        public IActionResult AddTask(AddTaskRequest taskRequest,Task taskType)
        {
            try
            {
                _service.addTaskRequestAndTaskTypeIfNotExists(taskRequest, taskType);
            }
            catch (SqlException e)
            {
                return BadRequest(e.Message);
            }

            return Ok("New task was added to database");
        }
    }
}