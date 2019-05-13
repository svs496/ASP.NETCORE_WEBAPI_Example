using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Contracts;
using TaskManager.Entities;

namespace TaskManager.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {

        readonly ILoggerManager _logger;

        readonly IDataRepository<User> _dataRepository;

        public UserController(ILoggerManager logger, IDataRepository<User> Repository)
        {
            _logger = logger;
            _dataRepository = Repository;
        }

        // GET: api/User
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                _logger.LogInfo("Inside Get All API Call.");
                IEnumerable<User> projects = _dataRepository.GetAll();
                return Ok(projects);
            }
            catch (Exception ex)
            {

                _logger.LogError($"GetAll API Call failed: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {

                User usr = _dataRepository.Get(id);

                if (usr == null)
                {
                    _logger.LogInfo($"Inside GetById : {id} not found");
                    return NotFound("The user record couldn't be found.");
                }

                return Ok(usr);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Get by Id API Call failed: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/User
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                if (user == null)
                {
                    _logger.LogInfo($"Inside Post : project is null");
                    return BadRequest("project is null.");
                }

                //Map is needed as input param has taskId null
                User newUser = new User
                {
                    FirstName = user.FirstName.ToUpper(),
                    LastName = user.LastName.ToUpper(),
                    EmployeeID = user.EmployeeID
                };

                _dataRepository.Add(newUser);

                return CreatedAtAction(nameof(Post), new { id = newUser.UserId }, newUser);
                // return Ok();
            }
            catch (Exception ex)
            {

                _logger.LogError($"Post API Call failed: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (user == null || id != user.UserId)
                {
                    _logger.LogInfo($"Inside Put : {id} not found");
                    return BadRequest("Id is null.");
                }

                _dataRepository.Update(user);
                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError($"Put API Call failed: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
