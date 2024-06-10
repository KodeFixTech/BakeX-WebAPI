using BakeX_WebAPI.DAL;
using BakeX_WebAPI.Models;
using BakeX_WebAPI.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BakeX_WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/controller")]
    public class JobSeekerController : Controller
    {

        private readonly SqlConnectionFactory _connectionFactory;

        private readonly IJobSeekerRepository _jobSeekerRepository;

        public JobSeekerController(SqlConnectionFactory connectionFactory, IJobSeekerRepository jobSeekerRepository)
        {
            _connectionFactory = connectionFactory;

            _jobSeekerRepository = jobSeekerRepository;
        }


        [HttpGet]
        [Route("getJobSeekerProfile/{phoneno}")]
        public async Task<IActionResult> GetJobSeekerProfile(string phoneno)
        {
            try
            {
                if (phoneno == null)
                {
                    return BadRequest("Phone no cannot be null");
                }

                var result = await _jobSeekerRepository.GetJobSeekerProfile(phoneno);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet]
        [Route("getRecommendedJobs/{profileId}")]
        public async Task<IActionResult> GetJobSeekerProfile(int profileId)
        {
            try
            {
                if (profileId == null)
                {
                    return BadRequest("ProfileId no cannot be null");
                }

                var result = await _jobSeekerRepository.GetRecommendedJobs(profileId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet]
        [Route("getJobs/{profileId}")]
        public async Task<IActionResult> GetJobs(int profileId)
        {
            try
            {
                if (profileId == null)
                {
                    return BadRequest("ProfileId no cannot be null");
                }

                var result = await _jobSeekerRepository.GetJobs(profileId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost]
        [Route("ApplyJobs")]
        public async Task<IActionResult> ApplyForJob(JobApplication application)
        {
            try
            {
                await _jobSeekerRepository.ApplyForJob(application);
                return Ok(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


    }
}
