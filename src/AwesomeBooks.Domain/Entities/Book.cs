using System.ComponentModel.DataAnnotations;

namespace AwesomeBooks.Domain.Entities
{
    public class Book : AggregateRoot, INamed
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]        
        public string Name { get; set; }

        public int PublishYear { get; set; }

        [MaxLength(255)]
        public string Authors { get; set; }

        public decimal Rating { get; set; }

        [MaxLength(1024)]
        public string ImageUri { get; set; }

        [MaxLength(1024)]
        public string AmazonUri { get; set; }

        [MaxLength(255)]
        public string ContentType { get; set; }

        [MaxLength(1024)]
        public string ContentUri { get; set; }

        [MaxLength(8000)]
        public string Reflection { get; set; }

        [Required]
        public virtual Category Category { get; set; }

        public override string GetEntityName()
        {
            return nameof(Book);
        }
    }
}
