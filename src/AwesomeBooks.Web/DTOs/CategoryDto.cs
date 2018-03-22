using AwesomeBooks.Domain.Entities;

namespace AwesomeBooks.Web.DTOs
{
    public class CategoryDto
    {
        public long Id { get; set; }
        public long? AreaId { get; set; }
        public string AreaName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public static CategoryDto ConvertFrom(Category category)
        {
            if (category == null)
            {
                return null;
            }
            return new CategoryDto
            {
                Id = category.Id,
                AreaId = category.CategoryArea?.Id,
                AreaName = category.CategoryArea?.Name,
                Name = category.Name,
                Description = category.Description
            };
        }
    }
}
