using AwesomeBooks.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AwesomeBooks.Domain.Integrations.Import
{
    public interface ICategoryAreaImporter
    {
        Task<ImportResult<CategoryArea>> Import(string csvContent);
    }
}
