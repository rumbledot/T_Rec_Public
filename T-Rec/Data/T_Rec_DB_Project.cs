using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using T_Rec.Models;

namespace T_Rec.Data
{
    public class T_Rec_DB_Project
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<T_Rec_DB_Project> Instance = new AsyncLazy<T_Rec_DB_Project>
        (
            async () =>
            {
                var instance = new T_Rec_DB_Project();
                CreateTableResult result = await Database.CreateTableAsync<Project>();
                return instance;
            }
        );

        public T_Rec_DB_Project()
        {
            Database = new SQLiteAsyncConnection(T_Rec_Controller.DatabasePath, T_Rec_Controller.Flags);
        }

        public Task<List<Project>> GetActiveProjects()
        {
            try
            {
                return GetAllProjects(ProjectStatus.active);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<List<Project>> GetAllProjects(ProjectStatus status = ProjectStatus.all)
        {
            List<Project> result = new List<Project>();

            return Database.QueryAsync<Project>($"SELECT P.*, C.NAME FROM PROJECT P JOIN COMPANY C ON C.COMPANY_ID=P.COMPANY_ID WHERE P.PROJECT_STATUS={(int)status}");
            
        }

        public IEnumerable<Project> GetProjects(ProjectStatus status) 
        {
            return (IEnumerable<Project>)Database.Table<Project>()
                .Where(p => (p.project_status == status))
                .OrderByDescending(p => (p.name));
        }

        public IEnumerable<Company> GetCompanies()
        {
            return (IEnumerable<Company>)Database.Table<Company>()
                .OrderByDescending(c => (c.name));
        }
    }
}
