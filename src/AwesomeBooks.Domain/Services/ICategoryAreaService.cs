using AwesomeBooks.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AwesomeBooks.Domain.Services
{
    public interface ICategoryAreaService
    {
        Task CreateAsync(CategoryArea categoryArea);
        Task<bool> DeleteAsync(long categoryAreaId);
        Task<IEnumerable<CategoryArea>> ReadAllAsync();
        Task<CategoryArea> ReadAsync(long categoryAreaId);
        Task<CategoryArea> ReadByNameAsync(string name);
        Task UpdateAsync(CategoryArea categoryArea);
    }
}
