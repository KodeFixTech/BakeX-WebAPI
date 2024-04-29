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
        private IBakeryOwnerRepository _ownerRepository;

        public AuthenticationController(IUserRepository userRepository, IBakeryOwnerRepository ownerRepository)
        {
            _userRepository = userRepository;
            _ownerRepository = ownerRepository;
        }

        [HttpPost]
        [Route("InsertUser")]
        public async Task<IActionResult> AddUserDetails(User user)
        {

            try
            {
                bool userAdded = await _userRepository.AddUserDetailsFromGoogleSignIn(user);

                return Ok(userAdded);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("InsertNonGoogleUser")]
        public async Task<IActionResult> AddNonGoogleUser(NonGoogleUser userData)
        {
            try
            {
                bool userAdded = await _userRepository.SignUpNonGoogleUser(userData);
                return Ok(userAdded);
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        [HttpPost]
        [Route("SignInNonGoogleUsers")]
        public async Task<IActionResult> SignInNonGoogleUsers(NonGoogleUser userData)
        {
            try
            {
                string token = await _userRepository.SignInNonGoogleUser(userData);

                if (token == "Invalid credentials")
                {
                    return BadRequest("Invalid credentials. Unable to sign in.");

                }
                else
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None
                    };

                    Response.Cookies.Append("access_token", token, cookieOptions);

                    return Ok("User signed in successfully.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }




        [HttpPost]
        [Route("IsBakeUser")]
        public async Task<IActionResult> IsBakeUser(String phoneno)
        {
            try
            {
                bool userAdded = await _ownerRepository.CheckBakeUser(phoneno);

                return Ok(userAdded);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
