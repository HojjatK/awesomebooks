using System;
using System.Collections.Generic;
using AwesomeBooks.Domain.Entities;
using AwesomeBooks.Domain.Integrations.Records;
using System.Linq;

namespace AwesomeBooks.Domain.Integrations.Export
{
    public class CategoryExporter : ICategoryExporter
    {
        private readonly ICsvRecordExtractor _recordExtractor;

        public CategoryExporter(ICsvRecordExtractor recordExtractor)
        {
            _recordExtractor = recordExtractor;
        }

        public string Export(IList<Category> categories)
        {
            return _recordExtractor.GetRecordsContent<CategoryRecord, CategoryRecordMap>(
                                    categories.Select(c => new CategoryRecord
                                                               {
                                                                  Name = c.Name,
                                                                  Description = c.Description,
                                                                  AreaName = c.CategoryArea?.Name
                                                               }));
        }
    }
}
