using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AwesomeBooks.Domain.Integrations
{
    public class CsvRecordExtractor : ICsvRecordExtractor
    {
        public IList<TRecord> GetRecords<TRecord, TRecordMap>(string content) where TRecordMap : ClassMap<TRecord>
        {
            var textReader = new StringReader(content);
            var csv = new CsvReader(textReader);
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.MissingFieldFound = null;
            csv.Configuration.ShouldSkipRecord = record =>
            {
                if (record.Any(s => s.Contains("\0")))
                {
                    return true;
                }
                return record.All(string.IsNullOrWhiteSpace);
            };
            csv.Configuration.RegisterClassMap<TRecordMap>();

            var records = csv.GetRecords<TRecord>().ToList();
            return records;
        }

        public string GetRecordsContent<TRecord, TRecordMap>(IEnumerable<TRecord> records) where TRecordMap : ClassMap<TRecord>
        {
            var textWriter = new StringWriter();
            var csv = new CsvWriter(textWriter);
            csv.Configuration.RegisterClassMap<TRecordMap>();
            csv.Configuration.QuoteAllFields = true;           
            csv.WriteRecords(records);
            textWriter.Flush();
            return textWriter.GetStringBuilder().ToString();
        }
    }
}
