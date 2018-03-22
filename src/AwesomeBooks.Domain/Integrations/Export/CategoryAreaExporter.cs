using AwesomeBooks.Domain.Entities;
using AwesomeBooks.Domain.Integrations.Records;
using System.Collections.Generic;
using System.Linq;

namespace AwesomeBooks.Domain.Integrations.Export
{
    public class CategoryAreaExporter : ICategoryAreaExporter
    {
        private readonly ICsvRecordExtractor _recordExtractor;

        public CategoryAreaExporter(ICsvRecordExtractor recordExtractor)
        {
            _recordExtractor = recordExtractor;
        }

        public string Export(IList<CategoryArea> categoryAreas)
        {
            return _recordExtractor.GetRecordsContent<CategoryAreaRecord, CategoryAreaRecordMap>(
                                    categoryAreas.Select(a => new CategoryAreaRecord
                                                      {
                                                         Name = a.Name,
                                                         Description = a.Description
                                                      }));
        }
    }
}
