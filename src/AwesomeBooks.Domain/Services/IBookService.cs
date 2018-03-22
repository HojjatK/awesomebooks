using AwesomeBooks.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AwesomeBooks.Domain.Services
{
    public interface IBookService
    {
        Task CreateAsync(Book book);
        Task<bool> DeleteAsync(long bookId);
        Task<IEnumerable<Book>> ReadAllAsync();
        Task<Book> ReadAsync(long bookId);
        Task<Book> ReadByNameAsync(string name, int publishYear, string authors);
        Task UpdateAsync(Book book);
    }
}