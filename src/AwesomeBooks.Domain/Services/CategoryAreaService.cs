using System.Collections.Generic;
using System.Threading.Tasks;
using AwesomeBooks.Domain.Entities;
using AwesomeBooks.Domain.EF;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using AwesomeBooks.Domain.Exceptions;

namespace AwesomeBooks.Domain.Services
{
    public class CategoryAreaService : ICategoryAreaService
    {
        private readonly DomainContext _context;       

        public CategoryAreaService(DomainContext context)
        {
            _context = context;            
        }

        public async Task CreateAsync(CategoryArea categoryArea)
        {
            ValidateDuplicateName(categoryArea);

            _context.Add(categoryArea);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(long categoryAreaId)
        {
            var categoryArea = await ReadAsync(categoryAreaId);
            if (categoryArea == null)
            {
                return false;
            }
            ValidateNoCategoryAssigned(categoryArea);

            _context.CategoryAreas.Remove(categoryArea);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CategoryArea>> ReadAllAsync()
        {
            var categoryAreas = await _context.CategoryAreas.ToListAsync();
            return categoryAreas;
        }

        public async Task<CategoryArea> ReadAsync(long categoryAreaId)
        {
            var categoryArea = await _context.CategoryAreas
                                     .Include(a => a.Categories)
                                     .SingleOrDefaultAsync(a => a.Id == categoryAreaId);
            return categoryArea;
        }

        public async Task<CategoryArea> ReadByNameAsync(string name)
        {
            var categoryArea = await _context.CategoryAreas
                                         .Include(a => a.Categories)
                                         .FirstOrDefaultAsync(b => b.Name == name);
            return categoryArea;
        }

        public async Task UpdateAsync(CategoryArea categoryArea)
        {
            ValidateDuplicateName(categoryArea);
            _context.CategoryAreas.Update(categoryArea);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!CategoryAreaExists(categoryArea.Id))
                {   
                    throw new EntityNotFoundException($"{categoryArea.GetEntityName()} with Id:{categoryArea.Id} not found", e);
                }
                throw;
            }
        }

        private bool CategoryAreaExists(long id)
        {
            return _context.CategoryAreas.Any(e => e.Id == id);
        }

        private void ValidateDuplicateName(CategoryArea categoryArea)
        {
            var id = categoryArea.Id;
            var name = categoryArea?.Name?.Trim();
            var sameNameAreaExists = _context.CategoryAreas
                                             .Any(c => c.Name == name && c.Id != id);
            if (sameNameAreaExists)
            {
                throw new EntityAlreadyExistsException($"{categoryArea?.GetEntityName()} with same name:'{name}' already exists.");
            }
        }

        private void ValidateNoCategoryAssigned(CategoryArea categoryArea)
        {
            var categoryAreaId = categoryArea.Id;
            var anyCategoryAssigned = _context.Categories.Any(c => c.CategoryArea.Id == categoryAreaId);
            if (anyCategoryAssigned)
            {
                throw new ValidationException($"There are one or more categories assigned to '{categoryArea?.Name}' area.");
            }
        }
    }
}
