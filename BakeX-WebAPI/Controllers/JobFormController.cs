using BakeX_WebAPI.DAL;
using BakeX_WebAPI.Models;
using BakeX_WebAPI.Repositories.Interface;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Data.SqlClient;

namespace BakeX_WebAPI.Controllers
{
   // [Authorize]
    [ApiController]
    [Route("api/v1/controller")]
    public class JobFormController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly SqlConnectionFactory _connectionFactory;
        private readonly IJobFormRepository   _jobFormRepository;
        private readonly IUserRepository _userRepository;

        public JobFormController(IJobFormRepository jobFormRepository, IUserRepository userRepository)
        {
         
            _jobFormRepository = jobFormRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("getJobCategory")]
        public async Task<IActionResult> getJobCategory()
        {
            try
            {
               var categories = await _jobFormRepository.GetJobCategory();
                return Ok(categories);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while fetching categories: {ex.Message}");
            }
           
        }





        [HttpGet]
        [Route("getEmploymentCategory")]
        public async Task<IActionResult> getEmploymentTypes()
        {
            try
            {
                var categories = await _jobFormRepository.getEmploymentTypes();
                return Ok(categories);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while fetching categories: {ex.Message}");
            }

        }

        [HttpPost]
        [Route("CreateJobPost")]

        public async Task<IActionResult> CreateJobPost(JobPost job)
        {
            try
            {
              var result=  await _jobFormRepository.CreateJobPost(job);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while fetching categories: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("getExpertiseCategory")]
        public async Task<IActionResult> getExpertiseTypes()
        {
            try
            {
                var categories = await _jobFormRepository.getExpertiseTypes();
                return Ok(categories);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while fetching categories: {ex.Message}");
            }

        }

        //[HttpPost]
        //[Route("InsertUser")]
        //public async Task<IActionResult> AddUserDetails(User user)
        //{
        //    try
        //    {
        //       bool userAdded = await _userRepository.AddUserDetailsFromGoogleSignIn(user);
        //        return Ok(200);
        //    }
        //    catch (Exception ex) {
        //        return StatusCode(500, ex.Message);
        //    }
        //}


        [HttpGet]
        [Route("getUserByEmail")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userRepository.GetUserFromEmail(email);
                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return NotFound("User not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while fetching user details: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("getDistrict")]
        public async Task<IActionResult> GetDistrict()
        {
            try
            {
                var distict = await _userRepository.GetDistricts();
                if (distict != null)
                {
                    return Ok(distict);
                }
                else
                {
                    return NotFound("User not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while fetching user details: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("getStates")]
        public async Task<IActionResult> GetStates()
        {
            try
            {
                var states = await _userRepository.GetStates();
                if (states != null)
                {
                    return Ok(states);
                }
                else
                {
                    return NotFound("User not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while fetching user details: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("InsertProfile")]
        public async Task<IActionResult> AddProfileDetails(Profile profile)
        {
            try
            {
                await _userRepository.SaveUserProfile(profile);
                return Ok(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
