using System.Collections.Generic;

namespace AwesomeBooks.Web.ViewModels
{
    public class BooksViewModel
    {
        public List<CategoryAreaItemViewModel> CategoryAreas { get; set; } = new List<CategoryAreaItemViewModel>();
    }
}
