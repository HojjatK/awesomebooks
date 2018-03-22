using AwesomeBooks.Domain.Entities;

namespace AwesomeBooks.Web.DTOs
{
    public class BookDto
    {
        public long Id { get; set; }
        public long? AreaId { get; set; }
        public string AreaName { get; set; }
        public long? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public int PublishYear { get; set; }
        public string Authors { get; set; }
        public decimal Rating { get; set; }
        public string ImageUri { get; set; }
        public string AmazonUri { get; set; }
        public string ContentType { get; set; }
        public string ContentUri { get; set; }        
        public string Reflection { get; set; }

        public static BookDto ConvertFrom(Book book)
        {
            if (book == null)
            {
                return null;
            }
            return new BookDto
            {
                Id = book.Id,
                AreaId = book.Category?.CategoryArea?.Id,
                AreaName = book.Category?.CategoryArea?.Name,
                CategoryId = book.Category?.Id,
                CategoryName = book.Category?.Name,
                Name = book.Name,
                PublishYear = book.PublishYear,
                Authors = book.Authors,
                Rating = book.Rating,
                ImageUri = book.ImageUri,
                AmazonUri = book.AmazonUri,
                ContentType = book.ContentType,
                ContentUri = book.ContentUri,
                Reflection = book.Reflection
            };
        }
    }
}
