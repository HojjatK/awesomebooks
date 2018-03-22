using CsvHelper.Configuration;
using System.Collections.Generic;

namespace AwesomeBooks.Domain.Integrations
{
    public interface ICsvRecordExtractor
    {
        IList<TRecord> GetRecords<TRecord, TRecordMap>(string content) where TRecordMap : ClassMap<TRecord>;
        string GetRecordsContent<TRecord, TRecordMap>(IEnumerable<TRecord> recoreds) where TRecordMap : ClassMap<TRecord>;
    }
}
