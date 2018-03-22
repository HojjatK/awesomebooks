using AwesomeBooks.Domain.Entities;
using AwesomeBooks.Domain.Integrations.Records;
using AwesomeBooks.Domain.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AwesomeBooks.Domain.Integrations.Import
{
    public class CategoryAreaImporter : ICategoryAreaImporter
    {
        private ICsvRecordExtractor _recordExtractor;
        private ICategoryAreaService _categoryAreaService;
        private ILogger<CategoryAreaImporter> _logger;

        public CategoryAreaImporter(
            ICategoryAreaService categoryAreaService,
            ICsvRecordExtractor recordExtractor,
            ILogger<CategoryAreaImporter> logger)
        {   
            _categoryAreaService = categoryAreaService;
            _recordExtractor = recordExtractor;
            _logger = logger;
        }

        public async Task<ImportResult<CategoryArea>> Import(string csvContent)
        {
            var result = new ImportResult<CategoryArea>
            {
                ImportedEntities = new List<CategoryArea>(),
                ErrorMessages = new List<string>()
            };

            int counter = 0;
            var records = _recordExtractor.GetRecords<CategoryAreaRecord, CategoryAreaRecordMap>(csvContent);            
            foreach (var record in records)
            {
                counter++;
                if (string.IsNullOrWhiteSpace(record.Name))
                {
                    AddError(result, $"[{counter}]: Area name is null or empty.");
                    continue;
                }

                var categoryArea = await _categoryAreaService.ReadByNameAsync(record.Name);                
                if (categoryArea != null)
                {
                    AddError(result, $"[{counter}]: Area with name: {record.Name} already exists.");                    
                    continue;
                }

                categoryArea = new CategoryArea
                {
                    Name = record.Name,
                    Description = record.Description
                };
                await _categoryAreaService.CreateAsync(categoryArea);
                result.ImportedEntities.Add(categoryArea);
            }

            return result;
        }

        private void AddError(ImportResult<CategoryArea> result, string errorMessage)
        {
            _logger.LogError(errorMessage);
            result.ErrorMessages.Add(errorMessage);
        }
    }
}
