using BakeX_WebAPI.DAL;
using BakeX_WebAPI.Models;
using BakeX_WebAPI.Repositories.Interface;
using Dapper;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BakeX_WebAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private SqlConnectionFactory _connection;
        private readonly IConfiguration _config;
        public UserRepository(SqlConnectionFactory connection, IConfiguration config)
        {
            _connection = connection;
            _config = config;
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


        public async Task<bool> SignUpNonGoogleUser(NonGoogleUser userData)
        {

            try
            {
                if (userData == null)
                {
                    throw new ArgumentNullException();
                }



                using (SqlConnection connection = _connection.CreateConnection())
                {
                    await connection.OpenAsync();

                    bool userExists = await connection.QueryFirstOrDefaultAsync<bool>("SELECT CASE WHEN EXISTS (SELECT 1 FROM NonGoogleUsers WHERE phone_number = @phoneNum) THEN 1 ELSE 0 END", new { phoneNum = userData.PhoneNum });


                    if (userExists)
                    {
                        return false;
                    }
                    else
                    {
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userData.Password);
                        userData.Password = hashedPassword;
                        await connection.ExecuteAsync("INSERT INTO NonGoogleUsers (phone_number, password) VALUES (@phoneNum, @password)", new { phoneNum = userData.PhoneNum, password = userData.Password });
                        return true;
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public async Task<String> SignInNonGoogleUser(NonGoogleUser userData)
        {
            try
            {
                if (userData == null)
                {
                    throw new ArgumentNullException();
                }
                using (SqlConnection connection = _connection.CreateConnection())
                {
                    await connection.OpenAsync();

                    var user = await connection.QueryFirstOrDefaultAsync("SELECT * FROM NonGoogleUsers WHERE phone_number = @phoneNum", new { phoneNum = userData.PhoneNum });

                    bool verifyPassword;

                    if (user != null)
                    {
                        verifyPassword = BCrypt.Net.BCrypt.Verify(userData.Password, user.password);
                        if (verifyPassword)
                        {
                            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                            var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
                                      _config["Jwt:Issuer"],
                                      null,
                                      expires: DateTime.Now.AddMinutes(120),
                                      signingCredentials: credentials);

                            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);


                            return token;
                        }
                        else
                        {
                            return "Invalid credentials";
                        }
                    }
                    else
                    {
                        return "Invalid credentials";
                    }
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
