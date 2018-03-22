using CsvHelper.Configuration;

namespace AwesomeBooks.Domain.Integrations.Records
{
    public class CategoryRecordMap : ClassMap<CategoryRecord>
    {
        public CategoryRecordMap()
        {
            Map(m => m.Name).Name("Name");            
            Map(m => m.Description).Name("Description");
            Map(m => m.AreaName).Name("Area Name");
        }
    }
}
