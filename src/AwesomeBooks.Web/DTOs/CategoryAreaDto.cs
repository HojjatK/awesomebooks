using AwesomeBooks.Domain.Entities;

namespace AwesomeBooks.Web.DTOs
{
    public class CategoryAreaDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public static CategoryAreaDto ConvertFrom(CategoryArea categoryArea)
        {
            if (categoryArea == null)
            {
                return null;
            }
            return new CategoryAreaDto
            {
                Id = categoryArea.Id,
                Name = categoryArea.Name,
                Description = categoryArea.Description
            };
        }

        //public static CategoryArea ConvertTo(CategoryAreaDto categoryAreaDto)
        //{
        //    if (categoryAreaDto == null)
        //    {
        //        return null;
        //    }
        //    return new CategoryArea
        //    {
        //        Id = categoryAreaDto.Id,
        //        Name = categoryAreaDto.Name,
        //        Description = categoryAreaDto.Description
        //    };
        //}
    }
}
