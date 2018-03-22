using AwesomeBooks.Domain.Entities;
using System.Collections.Generic;

namespace AwesomeBooks.Domain.Integrations.Export
{
    public interface ICategoryExporter
    {
        string Export(IList<Category> categories);
    }
}
