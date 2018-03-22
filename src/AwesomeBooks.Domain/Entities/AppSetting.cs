using System.ComponentModel.DataAnnotations;

namespace AwesomeBooks.Domain.Entities
{
    public class AppSetting : AggregateRoot
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(255)]
        public string Name { get; set; }

        public bool Installed { get; set; }

        public override string GetEntityName()
        {
            return nameof(AppSetting);
        }
    }
}
