using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AwesomeBooks.Domain.EF;
using AwesomeBooks.Domain.Entities;
using AwesomeBooks.Domain.Exceptions;

namespace AwesomeBooks.Domain.Services
{
    public class BookService : IBookService
    {
        private readonly DomainContext _context;

        public BookService(DomainContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Book book)
        {
            ValidateDuplicateBook(book);
            _context.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(long bookId)
        {
            var book = await ReadAsync(bookId);
            if (book == null)
            {
                return false;
            }            

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Book>> ReadAllAsync()
        {
            var books = await _context.Books
                                      .Include(b => b.Category)
                                      .Include(b => b.Category.CategoryArea)
                                      .ToListAsync();
            return books;
        }

        public async Task<Book> ReadAsync(long bookId)
        {
            var book = await _context.Books
                                     .Include(b => b.Category)
                                     .Include(b => b.Category.CategoryArea)
                                     .SingleOrDefaultAsync(c => c.Id == bookId);
            return book;
        }

        public async Task<Book> ReadByNameAsync(string name, int publishYear, string authors)
        {
            var book = await _context.Books
                                     .Include(b => b.Category)
                                     .Include(b => b.Category.CategoryArea)
                                     .FirstOrDefaultAsync(c => c.Name == name && 
                                                               c.PublishYear == publishYear &&
                                                               c.Authors == authors);
            return book;
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.Id))
                {
                    throw new EntityAlreadyExistsException($"{book.GetEntityName()} with Id:{book.Id} does not exist");
                }
                else
                {
                    throw;
                }
            }
        }

        private bool BookExists(long id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

        private void ValidateDuplicateBook(Book book)
        {
            var name = book.Name?.Trim();
            var publishYear = book.PublishYear;
            var authors = book.Authors?.Trim();
            
            var duplicateExits = _context.Books
                                         .Any(b => b.Name == name && b.PublishYear == publishYear && b.Authors == authors);
            if (duplicateExits)
            {
                throw new EntityAlreadyExistsException($"{book?.GetEntityName()} with same name:'{name}', publish year:'{publishYear}', authors:'{authors}' already exists.");
            }
        }
    }
}
