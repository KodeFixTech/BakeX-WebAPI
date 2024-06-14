using BakeX_WebAPI.Models;

namespace BakeX_WebAPI.Repositories.Interface
{
    public interface IBakeryOwnerRepository
    {
        public Task<bool> CreateBakeryOwnerProfile(BakeMember Bakemember);

        public Task<BakeMember> GetBakeMemberProfile(string phoneNo);
        
         public Task<BakeMember> getBakeMemberDetails(string phoneno);

        public Task<bool> CheckBakeUser(String phoneno);

        public Task<IEnumerable<JobPost>> GetJobPostByOwner(int Id);
    }
}
