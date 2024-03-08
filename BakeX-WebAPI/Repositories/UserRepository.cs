using BakeX_WebAPI.DAL;
using BakeX_WebAPI.Models;
using BakeX_WebAPI.Repositories.Interface;
using Dapper;
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
                if(user==null)
                {
                    throw new ArgumentNullException();


                }

             

                using (SqlConnection connection = _connection.CreateConnection())
                {
                   await connection.OpenAsync();

                    await connection.ExecuteAsync("InsertUser", new
                    {
                        Email=user.email, GoogleId= user.googleId, Name=user.name, ProfileImage=user.profileImage
                    },
                    commandType:CommandType.StoredProcedure
                    );

                    return true;
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
                using (SqlConnection connection = _connection.CreateConnection())
                {
                    await connection.OpenAsync();

                    var districts = await connection.QueryAsync<District>("getDistrict", commandType: CommandType.StoredProcedure);
                    return districts;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<IEnumerable<States>> GetStates()
        {
            try
            {
                using (SqlConnection connection = _connection.CreateConnection())
                {
                    await connection.OpenAsync();

                    var states = await connection.QueryAsync<States>("getStates", commandType: CommandType.StoredProcedure);
                    return states;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


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
