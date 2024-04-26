using BakeX_WebAPI.DAL;
using BakeX_WebAPI.Models;
using BakeX_WebAPI.Repositories.Interface;
using BakeX_WebAPI.Services;
using Dapper;
using Microsoft.OpenApi.Any;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace BakeX_WebAPI.Repositories
{
    public class BakeryOwnerRepository : IBakeryOwnerRepository
    {
        private SqlConnectionFactory _connection;
        private ImageDecoder _imageDecoder;
        public BakeryOwnerRepository(SqlConnectionFactory connection, ImageDecoder decoder)
        {
            _connection = connection;
            _imageDecoder = decoder;
        }

        public async Task<bool> CreateBakeryOwnerProfile(BakeMember bakeMember)
        {
            try
            {
                using (SqlConnection connection = _connection.CreateConnection())
                {
                    await connection.OpenAsync();

                    //  var decodedImage = _imageDecoder.DecodeBase64Image(nonBakeMember.OtherInformation.LogoUrl);

                    bakeMember.ProfileCreatedDate = DateTime.Now;
                    

                    var parameters = new
                    {
                        FirstName= bakeMember.FirstName,   
                        LastName= bakeMember.LastName,
                        Age = bakeMember.Age,
                        Gender = bakeMember.Gender,
                        PhoneNo = bakeMember.PhoneNo,
                        State = bakeMember.State,
                        District = bakeMember.District,
                        Place = bakeMember.Place,
                        Pincode = bakeMember.Pincode,
                        BusinessName = bakeMember.BusinessName,
                        BusinessPhone= bakeMember.BusinessPhone,
                        BusinessAddress = bakeMember.BusinessAddress,
                        FssaiNo = bakeMember.FssaiNo,
                        FssaiExpiryDate = bakeMember.FssaiExpiryDate,
                        ProfileCreatedDate = bakeMember.ProfileCreatedDate


                    };

                    await connection.ExecuteAsync("CreateBakeryOwnerProfile", parameters);

                    return true;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }


        public async Task<BakeMember> GetBakeMemberProfile(string phoneNo)
        {

            try
            {
                using (SqlConnection connection = _connection.CreateConnection())
                {
                   await connection.OpenAsync();


                    var parameters = new DynamicParameters();
                    parameters.Add("@PhoneNo", phoneNo);

                    var result =  await connection.QueryFirstOrDefaultAsync<BakeMember>("sp_GetNonBakeryOwnerProfile", parameters, commandType: CommandType.StoredProcedure);
                    return result; 
                }
            }

            catch(Exception ex)
            {
                throw new Exception(ex.Message);
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

                if (userExists == 0)
                {
                    return false;

                }
                else
                {
                    return true;
                }


            }



        }



    }
}
