using System.Collections.Generic;

namespace AwesomeBooks.Web.DTOs
{
    public class ImportResultDto
    {
        public int ErrorsCount { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
