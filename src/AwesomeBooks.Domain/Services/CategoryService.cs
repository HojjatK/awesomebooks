using System.Collections.Generic;
using System.Threading.Tasks;
using AwesomeBooks.Domain.EF;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using AwesomeBooks.Domain.Exceptions;
using AwesomeBooks.Domain.Entities;

namespace AwesomeBooks.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DomainContext _context;     

        public CategoryService(DomainContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Category category)
        {
            ValidateDuplicateName(category);
            _context.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(long categoryId)
        {
            var category = await ReadAsync(categoryId);
            if (category == null)
            {
                return false;
            }
            ValidateNoBookAssigned(category);

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Category>> ReadAllAsync()
        {
            var categories = await _context.Categories
                                           .Include(c => c.CategoryArea).ToListAsync();
            return categories;
        }

        public async Task<Category> ReadAsync(long categoryId)
        {
            var category = await _context.Categories                                     
                                         .Include(c => c.CategoryArea)
                                         .SingleOrDefaultAsync(c => c.Id == categoryId);
            return category;
        }

        public async Task<Category> ReadByNameAsync(string categoryAreaName, string categoryName)
        {   
            var category = await _context.Categories      
                                         .Include(c => c.CategoryArea)
                                         .FirstOrDefaultAsync(c => c.Name == categoryName && c.CategoryArea.Name == categoryAreaName);
            return category;
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!CategoryExists(category.Id))
                {
                    throw new EntityNotFoundException($"{category.GetEntityName()} with Id:{category.Id} not found", e);
                }
                throw;
            }
        }

        private bool CategoryExists(long id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

        private void ValidateDuplicateName(Category category)
        {
            var name = category?.Name?.Trim();
            var categoryAreaId = category?.CategoryArea?.Id;
            var sameNameExists = _context.Categories
                                             .Any(c => c.Name == name && c.CategoryArea.Id == categoryAreaId);
            if (sameNameExists)
            {
                throw new EntityAlreadyExistsException($"{category?.GetEntityName()} with same name:'{name}' already exists.");
            }
        }

        private void ValidateNoBookAssigned(Category category)
        {
            var categoryId = category.Id;
            var anyBooksAssigned = _context.Books.Any(b => b.Category.Id == categoryId);
            if (anyBooksAssigned)
            {
                throw new ValidationException($"There are one or more books assigned to '{category?.Name}' category.");
            }
        }
    }
}
