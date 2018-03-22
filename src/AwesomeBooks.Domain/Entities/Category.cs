using System.ComponentModel.DataAnnotations;

namespace AwesomeBooks.Domain.Entities
{
    public class Category : AggregateRoot, INamed
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        [Required]
        public virtual CategoryArea CategoryArea { get; set; }

        public override string GetEntityName()
        {
            return nameof(Category);
        }
    }
}
