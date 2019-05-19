using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Contracts;
using TaskManager.DataLayer;
using TaskManager.Entities;

namespace TaskManager.API.Controllers
{
    [Route("api/project")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        readonly ILoggerManager _logger;

        readonly IDataRepository<Project> _dataRepository;

        public ProjectController(ILoggerManager logger, IDataRepository<Project> Repository)
        {
            _logger = logger;
            _dataRepository = Repository;
        }

        //GET: api/Project
       [HttpGet]
        public IActionResult Get()
        {
            try
            {
                _logger.LogInfo("Inside Get All API Call.");
                IEnumerable<Project> projects = _dataRepository.GetAll();
                return Ok(projects);
            }
            catch (Exception ex)
            {

                _logger.LogError($"GetAll API Call failed: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }





        // GET: api/Project/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {

                Project proj = _dataRepository.Get(id);

                if (proj == null)
                {
                    _logger.LogInfo($"Inside GetById : {id} not found");
                    return NotFound("The project record couldn't be found.");
                }

                return Ok(proj);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Get by Id API Call failed: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/Project/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] Project project)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (project == null || id != project.ProjectId)
                {
                    _logger.LogInfo($"Inside Put : {id} not found");
                    return BadRequest("Id is null.");
                }


                if(project.IsSuspended)
                {
                    // Do not change status  of project to suspended it it has a In-orogress child task
                    if (_dataRepository.CanDeleteEntity(id))
                    {
                        return Conflict(new { customMessage = $" Cannot Suspend. Project '{project.ProjectName}' has in-progress tasks." });
                    }
                }

                _dataRepository.Update(project);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError($"Put API Call failed: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/Project
        [HttpPost]
        public IActionResult Post([FromBody] Project project)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                if (project == null)
                {
                    _logger.LogInfo($"Inside Post : project is null");
                    return BadRequest("project is null.");
                }

                //Map is needed as input param has taskId null
                Project newProj = new Project
                {
                    EndDate = project.EndDate.Date,
                    Priority = project.Priority,
                    StartDate = project.StartDate.Date,
                    ProjectName = project.ProjectName.ToUpper(),
                    UserId = project.UserId,
                    IsSuspended = project.IsSuspended
                };

                _dataRepository.Add(newProj);

                return CreatedAtAction(nameof(Post), new { id = newProj.UserId }, newProj);

            }
            catch (Exception ex)
            {

                _logger.LogError($"Post API Call failed: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/Project/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            try
            {

                Project task = _dataRepository.Get(id);

                if (task == null)
                {
                    _logger.LogInfo($"Inside Delete : {id} not found");
                    return NotFound("The project record couldn't be found.");
                }


                // DO do not delete Task which has child
                if (_dataRepository.CanDeleteEntity(id))
                {
                   //
                }

                _dataRepository.Delete(task);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Delete API Call failed: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
