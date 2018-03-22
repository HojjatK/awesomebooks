using AwesomeBooks.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AwesomeBooks.Domain.Integrations.Import
{
    public interface ICategoryImporter
    {
        Task<ImportResult<Category>> Import(string csvContent);
    }
}
