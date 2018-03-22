using CsvHelper.Configuration;

namespace AwesomeBooks.Domain.Integrations.Records
{
    public class CategoryAreaRecordMap : ClassMap<CategoryAreaRecord>
    {
        public CategoryAreaRecordMap()
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.Description).Name("Description");            
        }
    }
}
