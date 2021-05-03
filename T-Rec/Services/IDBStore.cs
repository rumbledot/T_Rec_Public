using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace T_Rec.Services
{
    public interface IDBStore<T>
    {
        Task<bool> AddAsync(T item);
        Task<bool> UpdateAsync(T item);
        Task<bool> DeleteAsync(int id);
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(bool forceRefresh = false);
    }
}
