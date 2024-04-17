using BakeX_WebAPI.DAL;
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
        private readonly IUserRepository _userRepository;

        public BakeryOwnerController(SqlConnectionFactory connectionFactory, IUserRepository userRepository)
        {
            _connectionFactory = connectionFactory;
   
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("getBakeOwner")]
        public async Task<IActionResult> getBakeOwnerDetails(string phoneno)
        {
            var result =  await _userRepository.getBakeMemberDetails(phoneno);
            return Ok(result);
        }
    }
}
