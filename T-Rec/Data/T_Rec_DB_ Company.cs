using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T_Rec.Models;

namespace T_Rec
{
    //https://docs.microsoft.com/en-us/xamarin/xamarin-forms/data-cloud/data/databases
    public partial class T_Rec_DB_Company
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<T_Rec_DB_Company> Instance = new AsyncLazy<T_Rec_DB_Company>
        (
            async () =>
            {
                var instance = new T_Rec_DB_Company();
                CreateTableResult result = await Database.CreateTableAsync<JobUnit>();
                return instance;
            }
        );

        public T_Rec_DB_Company()
        {
            Database = new SQLiteAsyncConnection(T_Rec_Controller.DatabasePath, T_Rec_Controller.Flags);
        }

        public Task<List<Company>> GetCompanies()
        {
            return Database.Table<Company>().ToListAsync();
        }

        public Task<Company> GetCompanyAsync(int id)
        {
            return Database.Table<Company>()
                .Where
                (
                    c => c.company_id == id
                )
                .FirstOrDefaultAsync();
        }

        public Task<int> SaveCompanyAsync(Company company)
        {
            if (company.company_id != 0)
            {
                return Database.UpdateAsync(company);
            }
            else
            {
                return Database.InsertAsync(company);
            }
        }

        public Task<int> DeleteCompanyAsync(Company company)
        {
            return Database.DeleteAsync(company);
        }
    }
}