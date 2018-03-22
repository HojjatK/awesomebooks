namespace AwesomeBooks.Domain.Integrations.Records
{
    public class BookRecord
    {
        public string Title { get; set; }
        public int PublishYear { get; set; }
        public string Authors { get; set; }
        public decimal Rating { get; set; }
        public string ImageUri { get; set; }
        public string AmazonUri { get; set; }
        public string ContentUri { get; set; }
        public string ContentType { get; set; }
        public string Reflection { get; set; }
        public string AreaName { get; set; }
        public string CategoryName { get; set; }
    }
}
