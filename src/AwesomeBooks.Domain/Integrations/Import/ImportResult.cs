using System.Collections;
using System.Collections.Generic;

namespace AwesomeBooks.Domain.Integrations.Import
{
    public class ImportResult<T>
    {
        public List<T> ImportedEntities { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
