using BakeX_WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BakeX_WebAPI.Repositories.Interface
{
    public interface IJobFormRepository
    {

        public Task<IEnumerable<JobCategory>> GetJobCategory();

        public Task<IEnumerable<EmploymentInformation>> getEmploymentTypes();

        public Task<IEnumerable<ExpertiseInformation>> getExpertiseTypes();
    }
}
