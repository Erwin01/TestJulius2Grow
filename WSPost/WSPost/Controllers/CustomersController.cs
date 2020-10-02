using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WSPost.Context;

namespace WSPost.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly PostDbContext _context;

        public CustomersController(PostDbContext context)
        {
            _context = context;

        }

        #region [ GET ALL USER LOGIN ]
        /// <summary>
        /// Get all users login 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult GetAllUserLogin()
        {
            try
            {
                var users = _context.Logins.ToList();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        #endregion
    }
}
