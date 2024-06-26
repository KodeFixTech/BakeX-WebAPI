﻿using BakeX_WebAPI.DAL;
using BakeX_WebAPI.Models;
using BakeX_WebAPI.Repositories.Interface;
using Dapper;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace BakeX_WebAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private SqlConnectionFactory _connection;
        public UserRepository(SqlConnectionFactory connection)
        {
            _connection = connection;
        }

        public async Task<bool> AddUserDetailsFromGoogleSignIn(User user)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                using (SqlConnection connection = _connection.CreateConnection())
                {
                    await connection.OpenAsync();

                    // Execute the CheckUserByEmail stored procedure to check if the user exists


                    int userExists = await connection.ExecuteScalarAsync<int>("CheckUserByMobile", new
                    {

                        MobileNo = user.MobileNumber,
                        AuthTypeID = user.AuthId,
                        GoogleId = user.GoogleId,
                    }, commandType: CommandType.StoredProcedure);

                    if (userExists == 0)
                    {
                        if (user.AuthId == 1)
                        {

                            await connection.ExecuteAsync("InsertUser", new
                            {
                                @MobileNumber = user.MobileNumber,
                                @AuthTypeId = user.AuthId,
                                @UserTypeId = user.UserTypeId,
                                @IsMobileVerified = user.IsMobileVerified,
                                @CreatedAt = DateTime.Now,
                                @GoogleId = user.GoogleId,
                            }, commandType: CommandType.StoredProcedure);

                            return true;

                        }
                        else if (user.AuthId == 2)
                        {
                            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
                            user.Password = hashedPassword;
                            await connection.ExecuteAsync("InsertUser", new
                            {
                                @MobileNumber = user.MobileNumber,
                                @AuthTypeId = user.AuthId,
                                @UserTypeId = user.UserTypeId,
                                @IsMobileVerified = user.IsMobileVerified,
                                @CreatedAt = DateTime.Now,
                                @Password = user.Password,
                            }, commandType: CommandType.StoredProcedure);

                            return true;

                        }

                        return true;

                    }
                    else
                    {
                        // User already exists
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public async Task<User> CheckUserExist(User user)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                User userExists = null; // Initialize userExists outside of the conditional blocks

                using (SqlConnection connection = _connection.CreateConnection())
                {
                    await connection.OpenAsync();

                    if (user.AuthId == 2)
                    {
                        string query = @"
                         SELECT uc.password 
                         FROM UserCredentials uc 
                         INNER JOIN Users us ON uc.UserId = us.Id
                         WHERE us.MobileNumber = @MobileNumber";

                        var password = await connection.QueryFirstOrDefaultAsync<string>(query, new { MobileNumber = user.MobileNumber });
                        if(password != null)
                        {
                            var comparePassword = BCrypt.Net.BCrypt.Verify(user.Password, password);
                            if (comparePassword == true)
                            {
                                var parameters = new
                                {
                                    MobileNo = user.MobileNumber,
                                    AuthTypeId = user.AuthId,
                                    GoogleId = user.GoogleId,
                                };

                                userExists = await connection.QueryFirstOrDefaultAsync<User>("CheckUserByMobile", parameters, commandType: CommandType.StoredProcedure);
                            }
                        }
                        
                    }
                    else
                    {
                        var parameters = new
                        {
                            MobileNo = user.MobileNumber,
                            AuthTypeId = user.AuthId,
                            GoogleId = user.GoogleId,
                        };

                        userExists = await connection.QueryFirstOrDefaultAsync<User>("CheckUserByMobile", parameters, commandType: CommandType.StoredProcedure);
                    }
                }

                return userExists; // Return userExists after the try-catch block
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }





        public async Task<User> GetUserFromEmail(String email)
        {
            try
            {
                if (email == null)
                {
                    throw new ArgumentNullException();
                }
                using (SqlConnection connection = _connection.CreateConnection())
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Email", email);

                    var result = await connection.QueryFirstOrDefaultAsync<User>("GetUserByEmail", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<IEnumerable<District>> GetDistricts()
        {
            try
            {
                IEnumerable<District> districts;
                using (SqlConnection connection = _connection.CreateConnection())
                {
                    await connection.OpenAsync();

                    districts = await connection.QueryAsync<District>("GetDistricts", commandType: CommandType.StoredProcedure);
                    return districts;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<District>> GetStates()
        {
            return await GetDistricts();
        }






        public async Task SaveUserProfile(Profile profile)
        {
            using (SqlConnection connection = _connection.CreateConnection())
            {
                await connection.OpenAsync();

                try
                {
                    var result = await connection.QueryFirstOrDefaultAsync<int>(
                     "InsertProfile",
                     new
                     {
                         FirstName = profile.FirstName,
                         LastName = profile.LastName,
                         MobileNo = profile.MobileNo,
                         Age = profile.Age,
                         Gender = profile.Gender,
                         State = profile.State,
                         District = profile.District,
                         Place = profile.Place,
                         ProfileCreatedDate = profile.ProfileCreatedDate,
                         EducationId = profile.EducationId,
                         ExperienceId = profile.ExperienceId,
                         Pincode = profile.Pincode

                     },
                     commandType: CommandType.StoredProcedure
                 );


                    foreach (int expertiseId in profile.ExpertiseIds)
                    {
                        // Create parameters for expertise ID and job post ID
                        var skillParameters = new DynamicParameters();
                        skillParameters.Add("@ExpertiseId", expertiseId);
                        skillParameters.Add("@ProfileId", result); // Assuming jobPost.Id is the newly inserted job post ID

                        // Execute the stored procedure to insert expertise ID and job post ID into JobPostSkillSet table
                        var rowsAffected = await connection.ExecuteAsync("InsertUserSkillSet", skillParameters, commandType: CommandType.StoredProcedure);

                        // Check if the insertion was successful

                    }


                    foreach (int jobTypeId in profile.JobTypeIds)
                    {
                        // Create parameters for expertise ID and job post ID
                        var skillParameters = new DynamicParameters();
                        skillParameters.Add("@EmploymentId", jobTypeId);
                        skillParameters.Add("@ProfileId", result); // Assuming jobPost.Id is the newly inserted job post ID

                        // Execute the stored procedure to insert expertise ID and job post ID into JobPostSkillSet table
                        var rowsAffected = await connection.ExecuteAsync("InsertUserJobPrefernce", skillParameters, commandType: CommandType.StoredProcedure);

                        // Check if the insertion was successful

                    }

                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }

            }
        }


    }
}
