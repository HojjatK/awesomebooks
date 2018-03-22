namespace AwesomeBooks.Web.ViewModels
{
    public class BookItemViewModel
    {
        public BookItemViewModel()
        {
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public int Year { get; set; }
        public decimal Rating { get; set; }
        public string AmazonUrl { get; set; }
        public string ImageUrl { get; set; }
        public string DownloadUrl { get; set; }
        public string Reflection { get; set; }
    }

}
