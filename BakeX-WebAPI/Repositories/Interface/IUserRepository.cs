using BakeX_WebAPI.Models;

namespace BakeX_WebAPI.Repositories.Interface
{
    public interface IUserRepository
    {
        public Task<bool> AddUserDetailsFromGoogleSignIn(User user);

        public Task<User> GetUserFromEmail(String email);

        public  Task<IEnumerable<District>> GetDistricts();

        public  Task<IEnumerable<District>> GetStates();

        public Task SaveUserProfile(Profile profile);


    

       

    }
}
