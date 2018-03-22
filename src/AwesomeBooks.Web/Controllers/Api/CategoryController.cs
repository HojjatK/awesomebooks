using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AwesomeBooks.Web.DTOs;
using AwesomeBooks.Domain.Services;
using AwesomeBooks.Domain.Entities;
using AwesomeBooks.Domain.Exceptions;
using AwesomeBooks.Utilities.Web;
using AwesomeBooks.Domain.Integrations.Import;
using AwesomeBooks.Domain.Integrations.Export;

namespace AwesomeBooks.Web.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/category")]
    public class CategoryController : Controller
    {
        private readonly IFileUploadUtility _fileUploadUtility;
        private readonly ICategoryImporter _categoryImporter;
        private readonly ICategoryExporter _categoryExporter;
        private readonly ICategoryAreaService _categoryAreaService;
        private readonly ICategoryService _categoryService;

        public CategoryController(
            IFileUploadUtility fileUploadUtility,
            ICategoryImporter categoryImporter,
            ICategoryExporter categoryExporter,
            ICategoryAreaService categoryAreaService,
            ICategoryService categoryService)
        {
            this._fileUploadUtility = fileUploadUtility;
            this._categoryImporter = categoryImporter;
            this._categoryExporter = categoryExporter;
            this._categoryAreaService = categoryAreaService;
            this._categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IEnumerable<CategoryDto>> Get()
        {
            var categories = await _categoryService.ReadAllAsync();
            return categories.Select(category => CategoryDto.ConvertFrom(category)).ToList();
        }

        [HttpGet("{id}", Name = "GetCategory")]
        public async Task<IActionResult> Get(long id)
        {
            var categoryDto = await _categoryService.ReadAsync(id);
            var result = CategoryDto.ConvertFrom(categoryDto);
            return new ObjectResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CategoryDto categoryDto)
        {
            if(!categoryDto.AreaId.HasValue)
            {
                throw new NullReferenceException("Category Area Id is Null");
            }

            var categoryArea = await _categoryAreaService.ReadAsync(categoryDto.AreaId.Value);
            if (categoryArea == null)
            {
                throw new EntityNotFoundException($"Category Area with Id: {categoryDto.AreaId.Value} not found.");
            }

            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                CategoryArea = categoryArea
            };
            await _categoryService.CreateAsync(category);

            categoryDto.Id = category.Id;
            categoryDto.AreaId = categoryArea?.Id;
            categoryDto.AreaName = categoryArea?.Name;
            return CreatedAtRoute("GetCategory", new { id = categoryDto.Id }, categoryDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody]CategoryDto categoryDto)
        {
            if (id != categoryDto.Id)
            {
                throw new ApplicationException($"Invalid Category Id:{id}");
            }
            if (!categoryDto.AreaId.HasValue)
            {
                throw new NullReferenceException("Category Area Id is Null");
            }
            var categoryArea = await _categoryAreaService.ReadAsync(categoryDto.AreaId.Value);
            if (categoryArea == null)
            {
                throw new EntityNotFoundException($"Category Area ({id}) not found.");
            }

            var category = await _categoryService.ReadAsync(id);
            if (category == null)
            {
                throw new EntityNotFoundException($"Category ({id}) not found.");
            }
            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;
            if (category.CategoryArea?.Id != categoryDto.AreaId)
            {
                category.CategoryArea = categoryArea;
            }
            await _categoryService.UpdateAsync(category);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _categoryService.DeleteAsync(id);
            return new NoContentResult();
        }

        [HttpPost("upload")]
        public async Task<ImportResultDto> Upload()
        {
            var fileContent = await _fileUploadUtility.GetUploadFileContent(Request);
            var importResult = await _categoryImporter.Import(fileContent);
            return new ImportResultDto
            {
                ErrorsCount = importResult.ErrorMessages.Count,
                ErrorMessages = importResult.ErrorMessages.Take(20).ToList(), // send max 20 error messages
            };
        }

        [HttpGet("download")]
        public async Task<IActionResult> Download()
        {
            var allCategories = await _categoryService.ReadAllAsync();
            var csvContent = _categoryExporter.Export(allCategories.ToList());
            var bytes = System.Text.Encoding.Default.GetBytes(csvContent);
            return File(new MemoryStream(bytes), "text/csv", "categories.csv");
        }
    }
}