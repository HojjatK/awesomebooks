using System.Collections.Generic;

namespace AwesomeBooks.Web.ViewModels
{
    public class CategoryAreaItemViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Ref { get; set; }
        public string Class { get; set; }
        public List<CategoryItemViewModel> Categories { get; set; } = new List<CategoryItemViewModel>();
    }

}
