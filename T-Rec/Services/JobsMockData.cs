using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T_Rec.Models;

namespace T_Rec.Services
{
    public class JobsMockData : IDataStore<JobUnit>
    {
        readonly List<JobUnit> items;

        public JobsMockData()
        {
            items = new List<JobUnit>()
            {
                new JobUnit { job_id = 0, project_id = 100, description="This is an item description one.", time_start = DateTime.Now, time_end = DateTime.Now.AddHours(1) },
                new JobUnit { job_id = 0, project_id = 101, description="This is an item description two.", time_start = DateTime.Now, time_end = DateTime.Now.AddHours(2) },
                new JobUnit { job_id = 0, project_id = 102, description="This is an item description three.", time_start = DateTime.Now, time_end = DateTime.Now.AddHours(3) },
                new JobUnit { job_id = 0, project_id = 103, description="This is an item description four.", time_start = DateTime.Now, time_end = DateTime.Now.AddHours(4) },
                new JobUnit { job_id = 0, project_id = 104, description="This is an item description five.", time_start = DateTime.Now, time_end = DateTime.Now.AddHours(5) },
                new JobUnit { job_id = 0, project_id = 105, description="This is an item description six.", time_start = DateTime.Now, time_end = DateTime.Now.AddHours(6) }
            };
        }

        public async Task<bool> AddItemAsync(JobUnit item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(JobUnit item)
        {
            var oldItem = items.Where((JobUnit arg) => arg.job_id == item.job_id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id_string)
        {
            int id = Convert.ToInt32(id_string);
            var oldItem = items.Where((JobUnit arg) => arg.job_id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<JobUnit> GetItemAsync(string id_string)
        {
            int id = Convert.ToInt32(id_string);
            return await Task.FromResult(items.FirstOrDefault(s => s.job_id == id));
        }

        public async Task<IEnumerable<JobUnit>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var oldItem = items.Where((JobUnit arg) => arg.job_id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<JobUnit> GetItemAsync(int id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.job_id == id));
        }
    }
}
