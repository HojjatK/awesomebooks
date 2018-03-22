using CsvHelper.Configuration;

namespace AwesomeBooks.Domain.Integrations.Records
{
    public class BookRecordMap : ClassMap<BookRecord>
    {
        public BookRecordMap()
        {
            Map(m => m.Title).Name("Title");
            Map(m => m.PublishYear).Name("Publish Year");
            Map(m => m.Authors).Name("Authors");
            Map(m => m.Rating).Name("Rating");
            Map(m => m.ImageUri).Name("Image Uri");
            Map(m => m.AmazonUri).Name("Amazon Uri");            
            Map(m => m.ContentUri).Name("Content Uri");
            Map(m => m.ContentType).Name("Content Type");
            Map(m => m.AreaName).Name("Area Name");
            Map(m => m.CategoryName).Name("Category Name");
            Map(m => m.Reflection).Name("Reflection");
        }
    }
}
