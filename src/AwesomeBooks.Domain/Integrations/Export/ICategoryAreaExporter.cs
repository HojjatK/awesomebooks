using System.Collections.Generic;
using AwesomeBooks.Domain.Entities;

namespace AwesomeBooks.Domain.Integrations.Export
{
    public interface ICategoryAreaExporter
    {
        string Export(IList<CategoryArea> categoryAreas);
    }
}
