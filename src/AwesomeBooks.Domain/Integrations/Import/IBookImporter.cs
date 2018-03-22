using AwesomeBooks.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AwesomeBooks.Domain.Integrations.Import
{
    public interface IBookImporter
    {
        Task<ImportResult<Book>> Import(string csvContent);
    }
}
