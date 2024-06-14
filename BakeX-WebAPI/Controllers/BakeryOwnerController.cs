using BakeX_WebAPI.DAL;
using BakeX_WebAPI.Models;
using BakeX_WebAPI.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BakeX_WebAPI.Controllers
{


    [ApiController]
    [Route("api/v1/controller")]
    public class BakeryOwnerController : Controller
    {

        private readonly SqlConnectionFactory _connectionFactory;
        private readonly IJobFormRepository _jobFormRepository;

        private readonly IBakeryOwnerRepository _ownerRepository;

        public BakeryOwnerController(SqlConnectionFactory connectionFactory, IBakeryOwnerRepository ownerRepository)
        {
            _connectionFactory = connectionFactory;

            _ownerRepository = ownerRepository;
        }

        [HttpPost]
        [Route("getBakeOwner")]
        public async Task<IActionResult> getBakeOwnerDetails(string phoneno)
        {
            var result = await _ownerRepository.getBakeMemberDetails(phoneno);
            return Ok(result);
        }



        [HttpPost]
        [Route("createBakeryOwner")]
        public async Task<IActionResult> CreateBakeryOwner(BakeMember nonBakeMember)
        {

            try
            {
                if (nonBakeMember == null)
                {
                    return BadRequest("NonBakeMember cannot be null");
                }

                var result = await _ownerRepository.CreateBakeryOwnerProfile(nonBakeMember);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpGet]
        [Route("getNonBakeryOwner/{phoneNo}")]
        public async Task<IActionResult> GetNonBakeryOwner(string phoneNo)
        {
            try
            {
                if (phoneNo == null)
                {
                    return BadRequest("Phone no cannot be null");
                }

                var result = await _ownerRepository.GetBakeMemberProfile(phoneNo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpGet]
        [Route("getJobPostByOwner/{Id}")]
        public async Task<IActionResult> GetJobPostByOwner(int Id)
        {
            try
            {
                if (Id== null)
                {
                    return BadRequest("Phone no cannot be null");
                }

                var result = await _ownerRepository.GetJobPostByOwner(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        

    }
}
