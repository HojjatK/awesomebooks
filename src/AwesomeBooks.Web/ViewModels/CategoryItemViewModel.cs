using System.Collections.Generic;

namespace AwesomeBooks.Web.ViewModels
{
    public class CategoryItemViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Ref { get; set; }
        public string Class { get; set; }
        public List<BookItemViewModel> Books { get; set; } = new List<BookItemViewModel>();
    }

}
