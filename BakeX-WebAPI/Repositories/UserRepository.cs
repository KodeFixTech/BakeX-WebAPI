using BakeX_WebAPI.DAL;
using BakeX_WebAPI.Models;
using BakeX_WebAPI.Repositories.Interface;
using Dapper;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace BakeX_WebAPI.Repositories
{
    public class UserRepository :IUserRepository
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
                    int userExists = await connection.ExecuteScalarAsync<int>("CheckUserByEmail", new
                    {
                        Email = user.email
                    }, commandType: CommandType.StoredProcedure);

                    if (userExists == 0)
                    {
                        // If user doesn't exist, insert the user
                        await connection.ExecuteAsync("InsertUser", new
                        {
                            Email = user.email,
                            GoogleId = user.googleId,
                            Name = user.name,
                            ProfileImage = user.profileImage
                        }, commandType: CommandType.StoredProcedure);

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


        public async Task<bool> CheckBakeUser(String phoneno)
        {
            if (phoneno == null)
            {
                throw new ArgumentNullException();
            }

            using (SqlConnection connection = _connection.CreateConnection())
            {
                await connection.OpenAsync();
                int userExists = await connection.ExecuteScalarAsync<int>("CheckBakeUserByPhone", new
                {
                    Phone = phoneno
                }, commandType: CommandType.StoredProcedure);

                if(userExists==0)
                {
                    return false;

                }
                else
                {
                    return true;
                }


            }


           
        }


        public async Task<BakeMember> getBakeMemberDetails(string phoneno)
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
                    parameters.Add("@Phone", phoneno);

                    var result = await connection.QueryFirstOrDefaultAsync<BakeMember>("GetBakeMemberByPhone", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }

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

                await connection.ExecuteAsync(
                    "InsertProfile",
                    new
                    {
                        FirstName = profile.FirstName,
                        LastName = profile.LastName,
                        MobileNo = profile.MobileNo,
                        Gender = profile.Gender,
                        State = profile.State,
                        City = profile.City,
                        Place = profile.Place,
                        Email = profile.Email
                    },
                    commandType: CommandType.StoredProcedure
                );
            }
        }
    

    }
}
