using AwesomeBooks.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AwesomeBooks.Domain.Services
{
    public interface ICategoryService
    {   
        Task CreateAsync(Category category);
        Task<bool> DeleteAsync(long categoryId);
        Task<IEnumerable<Category>> ReadAllAsync();
        Task<Category> ReadAsync(long categoryId);
        Task<Category> ReadByNameAsync(string categoryAreaName, string categoryName);
        Task UpdateAsync(Category category);
    }
}
