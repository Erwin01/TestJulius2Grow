using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WSPost.Context;
using WSPost.Logs;
using WSPost.Models;

namespace WSPost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        #region [ VARIABLES ]
        private readonly PostDbContext _context;
        private readonly IServiceLogger _serviceLogger;
        #endregion

        #region [ CONSTRUCTOR ]
        /// <summary>
        /// Constructor context database and servicelogger
        /// </summary>
        /// <param name="context"></param>
        /// <param name="serviceLogger"></param>
        public PostsController(PostDbContext context, IServiceLogger serviceLogger)
        {
            _context = context;
            _serviceLogger = serviceLogger;
        }
        #endregion

        #region [ GET ALL USERS ] //jhon Doe, Jane Doe 5 data
        /// <summary>
        /// Get all users 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllPosts()
        {
            try
            {
                var users = _context.Posts.ToList();
                _serviceLogger.SendHiddenMessageDetails();

                return Ok(users);
            }
            catch (Exception ex)
            {
                _serviceLogger.SendHiddenMessageError();
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        #endregion



        #region [ CREATE POST ]
        [HttpPost]
        public IActionResult CreatePost([FromBody] Post user)
        {
            try
            {
                if (user == null)
                {
                    _serviceLogger.SendHiddenMessageErrorQuery();
                    return BadRequest("User is null");
                }

                if (!ModelState.IsValid)
                {
                    _serviceLogger.SendHiddenMessageErrorQuery();
                    return BadRequest("Invalid model");
                }

                user.Id = Guid.NewGuid();
                _context.Add(user);
                _context.SaveChanges();

                _serviceLogger.SendHiddenMessageInformation();
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                _serviceLogger.SendHiddenMessageError();
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        #endregion
    }
}
