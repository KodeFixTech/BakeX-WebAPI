using BakeX_WebAPI.DAL;
using BakeX_WebAPI.Models;
using BakeX_WebAPI.Repositories.Interface;
using Dapper;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using BakeX_WebAPI.Services;

namespace BakeX_WebAPI.Repositories
{
    public class JobFormRepository : IJobFormRepository
    {

        private readonly SqlConnectionFactory _connection;
        //cache
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(3);
        private readonly ImageDecoder _imageDecoder;

        /*public JobFormRepository(SqlConnectionFactory connection)
        {
            _connection = connection;
        }*/
        public JobFormRepository(SqlConnectionFactory connection, IMemoryCache memoryCache, ImageDecoder imageDecoder)
        {
            _connection = connection;
            _memoryCache = memoryCache;
            _imageDecoder = imageDecoder;
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
                var categories = await connection.QueryAsync<EmploymentInformation>("SELECT * FROM JobType");
                return categories;
            }
        }


        /*public  async Task<IEnumerable<ExpertiseInformation>> getExpertiseTypes()
        {
            using (SqlConnection connection = _connection.CreateConnection())
            {
                var categories = await connection.QueryAsync<ExpertiseInformation>("select * from ExpertiseInformation");
                return categories;
            }
        }*/
        public async Task<IEnumerable<ExpertiseInformation>> getExpertiseTypes()
        {
            // Cache key for expertise information
            var cacheKey = "expertiseTypes";

            // Try to get expertise information from cache
            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<ExpertiseInformation> expertiseTypes))
            {
                using (SqlConnection connection = _connection.CreateConnection())
                {
                    expertiseTypes = await connection.QueryAsync<ExpertiseInformation>("select * from ExpertiseInformation");

                    // Set cache options
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = _cacheDuration
                    };

                    // Cache the data
                    _memoryCache.Set(cacheKey, expertiseTypes, cacheOptions);
                }
            }

            return expertiseTypes;
        }


        public async Task<bool> CreateJobPost(JobPost jobPost)
        {
            using (SqlConnection connection = _connection.CreateConnection())
            {
                // Open the connection
                await connection.OpenAsync();

                jobPost.CreatedDate = DateTime.Now;
                jobPost.IsActive = 'Y';
                // Begin a transaction
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Create a parameter object to pass values to the stored procedure
                        var parameters = new DynamicParameters();
                        parameters.Add("@PostedById", jobPost.PostedById);
                        parameters.Add("@Title", jobPost.Title);
                        parameters.Add("@JobTypeId", jobPost.JobTypeId);
                        parameters.Add("@CreatedDate", jobPost.CreatedDate);
                        parameters.Add("@ExperienceId", jobPost.ExperienceId);
                        parameters.Add("@BusinessId", jobPost.BusinessId);
                        parameters.Add("@JobDescription", jobPost.JobDescription);
                        parameters.Add("@Salary", jobPost.Salary);
                        parameters.Add("@DistrictId", jobPost.DistrictId);
                        parameters.Add("@IsActive", jobPost.IsActive);
             

                        // Check if ProfileImage is provided and add it to parameters
                        if (jobPost.ProfileImage != null)
                        {
                            var base64Image = _imageDecoder.DecodeBase64Image(jobPost.ProfileImage);
                            parameters.Add("@ProfileImage", base64Image);
                        }

                        // Execute the stored procedure to insert job post details
                        var result = await connection.QueryFirstOrDefaultAsync<int>("InsertJobPost", parameters, commandType: CommandType.StoredProcedure,transaction: transaction);
                     
                        // Check if the job post insertion was successful


                        // Insert expertise IDs into JobPostSkillSet table
                        foreach (int expertiseId in jobPost.ExpertiseIds)
                        {
                            // Create parameters for expertise ID and job post ID
                            var skillParameters = new DynamicParameters();
                            skillParameters.Add("@ExpertiseId", expertiseId);
                            skillParameters.Add("@JobPostId", result); // Assuming jobPost.Id is the newly inserted job post ID

                            // Execute the stored procedure to insert expertise ID and job post ID into JobPostSkillSet table
                          var  rowsAffected = await connection.ExecuteAsync("InsertJobPostSkillSet", skillParameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                            // Check if the insertion was successful
                         
                        }

                        // Commit the transaction
                        transaction.Commit();

                        // Return true if everything executed successfully
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        // Log the exception or handle it appropriately
                        Console.WriteLine($"An SQL error occurred: {ex.Message}");
                        transaction.Rollback();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        // Log the exception or handle it appropriately
                        Console.WriteLine($"An error occurred: {ex.Message}");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }





    }
}
