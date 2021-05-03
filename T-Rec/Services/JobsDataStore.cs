using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T_Rec.Models;

namespace T_Rec.Services
{
    public class JobsDataStore : IDBStore<JobUnit>
    {
        readonly List<JobUnit> jobs;

        public JobsDataStore()
        {
            jobs = new List<JobUnit>()
            {
                new JobUnit { job_id = Convert.ToInt32(Guid.NewGuid()), project_id = 1, description = "First  job", time_start=DateTime.Now, time_end=DateTime.Now.AddHours(1) },
                new JobUnit { job_id = Convert.ToInt32(Guid.NewGuid()), project_id = 1, description = "Second job", time_start=DateTime.Now, time_end=DateTime.Now.AddHours(2) },
                new JobUnit { job_id = Convert.ToInt32(Guid.NewGuid()), project_id = 1, description = "Third  job", time_start=DateTime.Now, time_end=DateTime.Now.AddHours(3) },
                new JobUnit { job_id = Convert.ToInt32(Guid.NewGuid()), project_id = 1, description = "Fourth job", time_start=DateTime.Now, time_end=DateTime.Now.AddHours(4) },
                new JobUnit { job_id = Convert.ToInt32(Guid.NewGuid()), project_id = 1, description = "Fifth  job", time_start=DateTime.Now, time_end=DateTime.Now.AddHours(5) },
                new JobUnit { job_id = Convert.ToInt32(Guid.NewGuid()), project_id = 1, description = "Sixth  job", time_start=DateTime.Now, time_end=DateTime.Now.AddHours(6) }
            };
        }

        public async Task<bool> AddAsync(JobUnit item)
        {
            jobs.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync(JobUnit item)
        {
            var oldItem = jobs.Where((JobUnit arg) => arg.job_id == item.job_id).FirstOrDefault();
            jobs.Remove(oldItem);
            jobs.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var oldItem = jobs.Where((JobUnit arg) => arg.job_id == id).FirstOrDefault();
            jobs.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<JobUnit> GetAsync(int id)
        {
            return await Task.FromResult(jobs.FirstOrDefault(s => s.job_id == id));
        }

        public async Task<IEnumerable<JobUnit>> GetAllAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(jobs);
        }
    }
}
