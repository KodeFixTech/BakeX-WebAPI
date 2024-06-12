using BakeX_WebAPI.Models;
using BakeX_WebAPI.Repositories;
using BakeX_WebAPI.Repositories.Interface;
using BakeX_WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BakeX_WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/controller")]
    public class AuthenticationController : Controller
    {
        private IUserRepository _userRepository;
        private IBakeryOwnerRepository _ownerRepository;
        private readonly AuthService _authService;

        public AuthenticationController(IUserRepository userRepository, IBakeryOwnerRepository ownerRepository, AuthService authService)
        {
            _userRepository = userRepository;
            _ownerRepository = ownerRepository;
            _authService = authService;

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

        [HttpPost]
        [Route("CheckUserExist")]
        public async Task<IActionResult> IsUserExist(User user)
        {
            try
            {
                User userDetails = await _userRepository.CheckUserExist(user);
                if (userDetails != null)
                {
                    if(userDetails.MobileNumber==null)
                    {
                        return Ok(userDetails);
                    }
                    var token = _authService.GenerateJwtToken(userDetails);
                    return Ok(new { token });
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
