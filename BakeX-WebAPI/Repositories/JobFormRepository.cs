using BakeX_WebAPI.DAL;
using BakeX_WebAPI.Models;
using BakeX_WebAPI.Repositories.Interface;
using Dapper;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace BakeX_WebAPI.Repositories
{
    public class JobFormRepository : IJobFormRepository
    {

        private readonly SqlConnectionFactory _connection;

        public JobFormRepository(SqlConnectionFactory connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<JobCategory>> GetJobCategory()
        {
            using (SqlConnection connection = _connection.CreateConnection())
            {
                var categories = await connection.QueryAsync<JobCategory>("SELECT * FROM JobCategory");
                return categories;
            }


        }


        public async Task<IEnumerable<EmploymentInformation>> getEmploymentTypes()
        {
            using (SqlConnection connection = _connection.CreateConnection())
            {
                var categories = await connection.QueryAsync<EmploymentInformation>("SELECT * FROM EmploymentInformation");
                return categories;
            }
        }


        public  async Task<IEnumerable<ExpertiseInformation>> getExpertiseTypes()
        {
            using (SqlConnection connection = _connection.CreateConnection())
            {
                var categories = await connection.QueryAsync<ExpertiseInformation>("select * from ExpertiseInformation");
                return categories;
            }
        }





    }
}
