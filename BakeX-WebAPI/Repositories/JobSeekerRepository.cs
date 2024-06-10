using BakeX_WebAPI.DAL;
using BakeX_WebAPI.Models;
using BakeX_WebAPI.Repositories.Interface;
using BakeX_WebAPI.Services;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace BakeX_WebAPI.Repositories
{
    public class JobSeekerRepository : IJobSeekerRepository
    {

        private SqlConnectionFactory _connection;
        private ImageDecoder _imageDecoder;
        public JobSeekerRepository(SqlConnectionFactory connection)
        {
            _connection = connection;
     
        }
        public async Task<JobSeeker> GetJobSeekerProfile(string phoneno)
        {
            try 
            {
                if (phoneno == null)
                {
                    throw new ArgumentNullException();
                }
                using (SqlConnection connection = _connection.CreateConnection())
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@MobileNo", phoneno);

                    var result = await connection.QueryFirstOrDefaultAsync<JobSeeker>("GetJobSeekerDetailsByPhone", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Job>> GetRecommendedJobs(int profileId)
        {
            try
            {
                if (profileId == null)
                {
                    throw new ArgumentNullException();
                }
                using (SqlConnection connection = _connection.CreateConnection())
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@ProfileId", profileId);

                    var result = await connection.QueryAsync<Job>("GetRecommendedJob", parameters, commandType: CommandType.StoredProcedure);
                    return result.AsList();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Job>> GetJobs(int profileId)
        {
            try
            {
                if (profileId == null)
                {
                    throw new ArgumentNullException();
                }
                using (SqlConnection connection = _connection.CreateConnection())
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@ProfileId", profileId);

                    var result = await connection.QueryAsync<Job>("GetJob", parameters, commandType: CommandType.StoredProcedure);
                    return result.AsList();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task ApplyForJob(JobApplication application)
        {
            try
            {
                using (SqlConnection connection = _connection.CreateConnection())
                {
                    await connection.OpenAsync();
                    await connection.ExecuteAsync("ApplyJob", application, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error applying for job.", ex); // Rethrow the exception
            }

        }
    }
}
