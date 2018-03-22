using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AwesomeBooks.Domain.Entities
{
    public class CategoryArea : AggregateRoot, INamed
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        public virtual ICollection<Category> Categories { get; private set; }

        public override string GetEntityName()
        {
            return nameof(CategoryArea);
        }
    }
}
