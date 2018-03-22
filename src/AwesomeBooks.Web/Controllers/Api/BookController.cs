using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using AwesomeBooks.Utilities.Web;
using AwesomeBooks.Domain.Integrations.Import;
using AwesomeBooks.Domain.Integrations.Export;
using AwesomeBooks.Domain.Services;
using AwesomeBooks.Web.DTOs;
using AwesomeBooks.Domain.Exceptions;
using AwesomeBooks.Domain.Entities;

namespace AwesomeBooks.Web.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/book")]
    public class BookController : Controller
    {
        private readonly IFileUploadUtility _fileUploadUtility;
        private readonly IBookImporter _bookImporter;
        private readonly IBookExporter _bookExporter;
        private readonly ICategoryAreaService _categoryAreaService;
        private readonly ICategoryService _categoryService;
        private readonly IBookService _bookService;

        public BookController(
            IFileUploadUtility fileUploadUtility,
            IBookImporter bookImporter,
            IBookExporter bookExporter,
            ICategoryAreaService categoryAreaService,
            ICategoryService categoryService,
            IBookService bookService)
        {
            this._fileUploadUtility = fileUploadUtility;
            this._bookImporter = bookImporter;
            this._bookExporter = bookExporter;
            this._categoryAreaService = categoryAreaService;
            this._categoryService = categoryService;
            this._bookService = bookService;
        }

        [HttpGet]
        public async Task<IEnumerable<BookDto>> Get()
        {
            var books = await _bookService.ReadAllAsync();
            return books.Select(book => BookDto.ConvertFrom(book)).ToList();
        }

        [HttpGet("{id}", Name = "GetBook")]
        public async Task<IActionResult> Get(long id)
        {
            var bookDto = await _bookService.ReadAsync(id);
            var result = BookDto.ConvertFrom(bookDto);
            return new ObjectResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]BookDto bookDto)
        {
            if (!bookDto.AreaId.HasValue)
            {
                throw new NullReferenceException("Category Area Id is Null");
            }
            if (!bookDto.CategoryId.HasValue)
            {
                throw new NullReferenceException("Category Id is Null");
            }

            var category = await _categoryService.ReadAsync(bookDto.CategoryId.Value);
            if (category == null)
            {
                throw new EntityNotFoundException($"Category Area with Id: {bookDto.AreaId.Value} not found.");
            }
            if (bookDto.AreaId != category.CategoryArea?.Id)
            {
                throw new ApplicationException("Category Area Id is invalid");
            }

            var book = new Book
            {
                Name = bookDto.Name,
                PublishYear = bookDto.PublishYear,
                Authors = bookDto.Authors,
                Rating = bookDto.Rating,
                ImageUri = bookDto.ImageUri,
                AmazonUri = bookDto.AmazonUri,
                ContentType = bookDto.ContentType,
                ContentUri = bookDto.ContentUri,
                Reflection = bookDto.Reflection,
                Category = category
            };
            await _bookService.CreateAsync(book);

            bookDto.Id = category.Id;            
            return CreatedAtRoute("GetBook", new { id = bookDto.Id }, bookDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody]BookDto bookDto)
        {
            if (id != bookDto.Id)
            {
                throw new ApplicationException($"Invalid Book Id:{id}");
            }
            if (!bookDto.AreaId.HasValue)
            {
                throw new NullReferenceException("Category Area Id is Null");
            }
            if (!bookDto.CategoryId.HasValue)
            {
                throw new NullReferenceException("Category Id is Null");
            }

            var category = await _categoryService.ReadAsync(bookDto.CategoryId.Value);
            if (category == null)
            {
                throw new EntityNotFoundException($"Category {id}) not found.");
            }
            if (bookDto.AreaId != category.CategoryArea?.Id)
            {
                throw new ApplicationException("Category Area Id is invalid");
            }

            var book = await _bookService.ReadAsync(id);
            if (book == null)
            {
                throw new EntityNotFoundException($"Book ({id}) not found.");
            }

            book.Name = bookDto.Name;
            book.PublishYear = bookDto.PublishYear;
            book.Authors = bookDto.Authors;
            book.Rating = bookDto.Rating;
            book.ImageUri = bookDto.ImageUri;
            book.AmazonUri = bookDto.AmazonUri;
            book.ContentType = bookDto.ContentType;
            book.ContentUri = bookDto.ContentUri;
            book.Reflection = bookDto.Reflection;
            if (book.Category?.Id != bookDto.CategoryId)
            {
                book.Category = category;
            }
            await _bookService.UpdateAsync(book);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _bookService.DeleteAsync(id);
            return new NoContentResult();
        }

        [HttpPost("upload")]
        public async Task<ImportResultDto> Upload()
        {
            var fileContent = await _fileUploadUtility.GetUploadFileContent(Request);
            var importResult = await _bookImporter.Import(fileContent);
            return new ImportResultDto
            {
                ErrorsCount = importResult.ErrorMessages.Count,
                ErrorMessages = importResult.ErrorMessages.Take(20).ToList(), // send max 20 error messages
            };
        }

        [HttpGet("download")]
        public async Task<IActionResult> Download()
        {
            var allBooks = await _bookService.ReadAllAsync();
            var csvContent = _bookExporter.Export(allBooks.ToList());
            var bytes = System.Text.Encoding.Default.GetBytes(csvContent);
            return File(new MemoryStream(bytes), "text/csv", "books.csv");
        }
    }
}