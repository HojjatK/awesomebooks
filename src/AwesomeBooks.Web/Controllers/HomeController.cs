using AwesomeBooks.Domain.Services;
using AwesomeBooks.Web.ViewModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeBooks.Web.Controllers
{
    [Route("")]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ICategoryAreaService _categoryAreaService;
        private readonly ICategoryService _categoryService;
        private readonly IBookService _bookService;

        public HomeController(
            ICategoryAreaService categoryAreaService,
            ICategoryService categoryService,
            IBookService bookService)
        {
            _categoryAreaService = categoryAreaService;
            _categoryService = categoryService;
            _bookService = bookService;
        }

        [Route("")]        
        public async Task<IActionResult> Index()
        {
            var vm = new BooksViewModel();
            var allBooks = await _bookService.ReadAllAsync();
            var allCategories = await _categoryService.ReadAllAsync();            
            var allCategoryAreas = await _categoryAreaService.ReadAllAsync();
            
            foreach(var area in allCategoryAreas.OrderBy(ca => ca.Name))
            {
                var areaVm = new CategoryAreaItemViewModel
                {
                    Id = area.Id,
                    Name = area.Name,
                    Ref = $"area-{area.Id}"
                };
                var categories = allCategories.Where(c => c.CategoryArea?.Id == area.Id)
                                              .OrderBy(c => c.Name)
                                              .ToList();
                foreach(var category in categories)
                {
                    var categoryVm = new CategoryItemViewModel
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Ref = $"category-{category.Id}"
                    };
                    var books = allBooks.Where(b => b.Category?.Id == category.Id)
                                        .OrderByDescending(b => b.PublishYear)
                                        .ThenBy(b => b.Name)
                                        .ToList();
                    foreach(var book in books)
                    {
                        var bookVm = new BookItemViewModel
                        {
                            Id = book.Id,
                            Title = book.Name,
                            Year = book.PublishYear,
                            Authors = book.Authors,
                            Rating = book.Rating,
                            ImageUrl = book.ImageUri,
                            AmazonUrl = book.AmazonUri,
                            DownloadUrl = book.ContentUri,
                            Reflection = book.Reflection
                        };
                        categoryVm.Books.Add(bookVm);
                    }
                    areaVm.Categories.Add(categoryVm);
                }
                vm.CategoryAreas.Add(areaVm);
            }

            return View(vm);
        }

        [Route("/Home/Error")]
        public IActionResult Error()
        {
            // Get the details of the exception that occurred
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionFeature != null)
            {
                // Get which route the exception occurred at
                string routeWhereExceptionOccurred = exceptionFeature.Path;

                // Get the exception that occurred
                Exception exceptionThatOccurred = exceptionFeature.Error;

                // Do something with the exception
                // Log it with Serilog?
                // Send an e-mail, text, fax, or carrier pidgeon?  Maybe all of the above?
                // Whatever you do, be careful to catch any exceptions, otherwise you'll end up with a blank page and throwing a 500
            }

            return View();
        }
    }
}