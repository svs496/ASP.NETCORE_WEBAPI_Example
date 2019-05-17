using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Contracts;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentTaskController : ControllerBase
    {
        readonly ILoggerManager _logger;

        readonly IDataRepository<Entities.Task> _dataRepository;

        public ParentTaskController(ILoggerManager logger, IDataRepository<Entities.Task> Repository)
        {
            _logger = logger;
            _dataRepository = Repository;
        }

        // GET api/Task
        [HttpGet]
        public IActionResult GetParentTasks()
        {
            try
         {
                _logger.LogInfo("Inside GetParentTask.");
                IEnumerable<Entities.Task> parentTasks = _dataRepository.GetParentTasks();
                return Ok(parentTasks);
            }
            catch (Exception ex)
            {

                _logger.LogError($"GetParentTask API Call failed: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}