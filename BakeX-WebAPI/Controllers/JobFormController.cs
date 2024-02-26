using BakeX_WebAPI.DAL;
using BakeX_WebAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Data.SqlClient;

namespace BakeX_WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/controller")]
    public class JobFormController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly SqlConnectionFactory _connectionFactory;

        public JobFormController(IConfiguration configuration, SqlConnectionFactory connectionFactory)
        {
            _configuration = configuration;
            _connectionFactory = connectionFactory;
        }

        [HttpGet]
        [Route("getJobCategory")]
        public async Task<IActionResult> getJobCategory()
        {
            try
            {
                using SqlConnection connection = _connectionFactory.CreateConnection();

                var categories =  await connection.QueryAsync<JobCategory>("select * from  JobCategory ");

                return Ok(categories);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while fetching categories: {ex.Message}");
            }
           
        }

        [HttpPost]
        [Route("addEmployee")]
        public async Task<IActionResult> addEmployeeDetails(Employee employee)
        {
            try
            {

                using SqlConnection connection = _connectionFactory.CreateConnection();
                await connection.OpenAsync();

                string query = "INSERT INTO Employee (EmployeeName,Age,CategoryId) VALUES (@EmployeeName, @Age,@CategoryId)";

                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                command.Parameters.AddWithValue("@Age", employee.Age);
                command.Parameters.AddWithValue("@CategoryId", employee.CategoryId);
             

                await command.ExecuteNonQueryAsync();

                return Ok("Employee added successfully.");
            }
            catch (Exception ex) {

                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding the employee: {ex.Message}");


            }

         }
    }
}
