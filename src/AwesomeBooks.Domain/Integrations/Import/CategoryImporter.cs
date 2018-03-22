using AwesomeBooks.Domain.Entities;
using AwesomeBooks.Domain.Integrations.Records;
using AwesomeBooks.Domain.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AwesomeBooks.Domain.Integrations.Import
{
    public class CategoryImporter : ICategoryImporter
    {
        private ICsvRecordExtractor _recordExtractor;
        private ICategoryAreaService _categoryAreaService;
        private ICategoryService _categoryService;
        private ILogger<CategoryAreaImporter> _logger;

        public CategoryImporter(
            ICategoryAreaService categoryAreaService,
            ICategoryService categoryService,
            ICsvRecordExtractor recordExtractor,
            ILogger<CategoryAreaImporter> logger)
        {   
            _categoryAreaService = categoryAreaService;
            _categoryService = categoryService;
            _recordExtractor = recordExtractor;
            _logger = logger;
        }

        public async Task<ImportResult<Category>> Import(string csvContent)
        {
            var result = new ImportResult<Category>
            {
                ImportedEntities = new List<Category>(),
                ErrorMessages = new List<string>()
            };

            int counter = 0;
            var records = _recordExtractor.GetRecords<CategoryRecord, CategoryRecordMap>(csvContent);
            foreach (var record in records)
            {
                counter++;
                if(string.IsNullOrWhiteSpace(record.AreaName))
                {
                    AddError(result, $"[{counter}]: Area name is null or empty.");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(record.Name))
                {
                    AddError(result, $"[{counter}]: Category name is null or empty.");                    
                    continue;
                }

                var categoryArea = await _categoryAreaService.ReadByNameAsync(record.AreaName);
                if (categoryArea == null)
                {
                    AddError(result, $"[{counter}]: Area with name: {record.AreaName} does not exist.");
                    continue;
                }

                var category = await _categoryService.ReadByNameAsync(record.AreaName, record.Name);
                if (category != null)
                {
                    AddError(result, $"[{counter}]: Category with name: {record.Name} already exists in Area: {record.AreaName}.");
                    continue;
                }

                category = new Category
                {
                    Name = record.Name,
                    Description = record.Description,
                    CategoryArea = categoryArea
                };
                await _categoryService.CreateAsync(category);
                result.ImportedEntities.Add(category);
            }

            return result;
        }

        private void AddError(ImportResult<Category> result, string errorMessage)
        {
            _logger.LogError(errorMessage);
            result.ErrorMessages.Add(errorMessage);
        }
    }
}
