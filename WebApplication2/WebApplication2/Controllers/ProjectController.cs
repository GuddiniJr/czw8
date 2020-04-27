using System.Collections.Generic;
using System.Data.SqlClient;
using WebApplication2.DTOs.Responses;
using WebApplication2.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly IDbService _service;

        public ProjectController(IDbService dbService)
        {
            this._service = dbService;
        }

        [HttpGet("{idProject}")]
        public IActionResult GetProject(string idProject)
        {
            if (idProject == null)
            {
                return BadRequest("Missing id of the team");
            }

            List<ProjectInfoResponse> response;
            try
            {
                response = _service.GetInfoAboutProject(idProject);
            }
            catch (SqlException e)
            {
                return BadRequest(e.Message);
            }

            return Ok(response);
        }

     
    }
}