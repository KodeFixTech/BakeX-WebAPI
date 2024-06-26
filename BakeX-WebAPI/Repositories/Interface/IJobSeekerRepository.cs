﻿using BakeX_WebAPI.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BakeX_WebAPI.Repositories.Interface
{
    public interface IJobSeekerRepository
    {

        public Task<JobSeeker> GetJobSeekerProfile(string phoneno);

        public Task<List<RecommendedJob>> GetRecommendedJobs(int profileId);

        public Task ApplyForJob(JobApplication application);
    }
}
