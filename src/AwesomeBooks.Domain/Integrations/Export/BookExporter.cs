using System.Linq;
using System.Collections.Generic;
using AwesomeBooks.Domain.Entities;
using AwesomeBooks.Domain.Integrations.Records;

namespace AwesomeBooks.Domain.Integrations.Export
{
    public class BookExporter : IBookExporter
    {
        private readonly ICsvRecordExtractor _recordExtractor;

        public BookExporter(ICsvRecordExtractor recordExtractor)
        {
            _recordExtractor = recordExtractor;
        }

        public string Export(IList<Book> books)
        {
            return _recordExtractor.GetRecordsContent<BookRecord, BookRecordMap>(
                                    books.Select(b => new BookRecord
                                    {
                                        Title = b.Name,
                                        PublishYear = b.PublishYear,
                                        Authors = b.Authors,
                                        Rating = b.Rating,
                                        ImageUri = b.ImageUri,
                                        AmazonUri = b.AmazonUri,
                                        ContentUri = b.ContentUri,
                                        ContentType = b.ContentType,
                                        Reflection = b.Reflection,                                   
                                        AreaName = b.Category?.CategoryArea?.Name,
                                        CategoryName = b.Category?.Name
                                    }));
        }
    }
}
