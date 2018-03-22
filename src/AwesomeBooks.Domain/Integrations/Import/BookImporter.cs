using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AwesomeBooks.Domain.Entities;
using AwesomeBooks.Domain.Services;
using AwesomeBooks.Domain.Integrations.Records;

namespace AwesomeBooks.Domain.Integrations.Import
{
    public class BookImporter : IBookImporter
    {
        private ICsvRecordExtractor _recordExtractor;
        private ICategoryAreaService _categoryAreaService;
        private ICategoryService _categoryService;
        private IBookService _bookService;
        private ILogger<CategoryAreaImporter> _logger;

        public BookImporter(
            ICategoryAreaService categoryAreaService,
            ICategoryService categoryService,
            IBookService bookService,
            ICsvRecordExtractor recordExtractor,
            ILogger<CategoryAreaImporter> logger)
        {
            _categoryAreaService = categoryAreaService;
            _categoryService = categoryService;
            _bookService = bookService;
            _recordExtractor = recordExtractor;
            _logger = logger;
        }

        public async Task<ImportResult<Book>> Import(string csvContent)
        {
            var result = new ImportResult<Book>
            {
                ImportedEntities = new List<Book>(),
                ErrorMessages = new List<string>()
            };

            var counter = 0;
            var records = _recordExtractor.GetRecords<BookRecord, BookRecordMap>(csvContent);
            foreach (var record in records)
            {
                counter++;
                if (RecordInvalid(counter, result, record))
                {
                    continue;
                }

                var categoryArea = await _categoryAreaService.ReadByNameAsync(record.AreaName);
                if (categoryArea == null)
                {
                    AddError(result, $"[{counter}]: Area with name: {record.AreaName} does not exist.");
                    continue;
                }

                var category = await _categoryService.ReadByNameAsync(record.AreaName, record.CategoryName);
                if (category == null)
                {
                    AddError(result, $"[{counter}]: Category with name: {record.CategoryName} does not exist in Area: {record.AreaName}.");
                    continue;
                }

                var book = await _bookService.ReadByNameAsync(record.Title, record.PublishYear, record.Authors);
                if (book != null)
                {
                    AddError(result, $"[{counter}]: Book with title:'{record.Title}', publishYear:'{record.PublishYear}', authors:'{record.Authors}' already exists.");
                    continue;
                }

                book = new Book
                {
                    Name = record.Title,
                    PublishYear = record.PublishYear,
                    Authors = record.Authors,
                    Rating = record.Rating,
                    ImageUri = record.ImageUri,
                    AmazonUri = record.AmazonUri,
                    ContentUri = record.ContentUri,
                    ContentType = record.ContentType,
                    Reflection = record.Reflection,
                    Category = category
                };
                await _bookService.CreateAsync(book);

                result.ImportedEntities.Add(book);
            }

            return result;
        }

        private bool RecordInvalid(int counter, ImportResult<Book> result, BookRecord record)
        {
            if (string.IsNullOrWhiteSpace(record.AreaName))
            {
                AddError(result, $"[{counter}]: Area name is null or empty.");
                return true;
            }
            if (string.IsNullOrWhiteSpace(record.CategoryName))
            {
                AddError(result, $"[{counter}]: Category name is null or empty.");                
                return true;
            }
            if (string.IsNullOrWhiteSpace(record.Title))
            {
                AddError(result, $"[{counter}]: Title is null or empty.");
                return true;
            }
            return false;
        }

        private void AddError(ImportResult<Book> result, string errorMessage)
        {
            _logger.LogError(errorMessage);
            result.ErrorMessages.Add(errorMessage);
        }
    }
}
