using BakeX_WebAPI.Models;
using BakeX_WebAPI.Repositories;
using BakeX_WebAPI.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BakeX_WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/controller")]
    public class AuthenticationController : Controller
    {
        private IUserRepository _userRepository;

        public AuthenticationController(IUserRepository userRepository) {
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("InsertUser")]
        public async Task<IActionResult> AddUserDetails(User user)
        {
            try
            {
                string userAdded = await _userRepository.AddUserDetailsFromGoogleSignIn(user);
                return Ok(userAdded);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
