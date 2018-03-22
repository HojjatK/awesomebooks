using System.Collections.Generic;
using AwesomeBooks.Domain.Entities;

namespace AwesomeBooks.Domain.Integrations.Export
{
    public interface IBookExporter
    {
        string Export(IList<Book> books);
    }
}
