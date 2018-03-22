using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AwesomeBooks.Web.DTOs;
using AwesomeBooks.Domain.Services;
using AwesomeBooks.Domain.Exceptions;
using AwesomeBooks.Utilities.Web;
using AwesomeBooks.Domain.Integrations.Import;
using AwesomeBooks.Domain.Integrations.Export;
using AwesomeBooks.Domain.Entities;

namespace AwesomeBooks.Web.Controllers.Api
{   
    [Produces("application/json")]
    [Route("api/category-area")]
    public class CategoryAreaController : Controller
    {
        private readonly IFileUploadUtility _fileUploadUtility;
        private readonly ICategoryAreaImporter _categoryAreaImporter;
        private readonly ICategoryAreaService _categoryAreaService;
        private readonly ICategoryAreaExporter _categoryAreaExporter;

        public CategoryAreaController(
            IFileUploadUtility fileUploadUtility,
            ICategoryAreaImporter categoryAreaImporter,
            ICategoryAreaExporter categoryAreaExporter,
            ICategoryAreaService categoryAreaService)
        {
            _fileUploadUtility = fileUploadUtility;
            _categoryAreaImporter = categoryAreaImporter;
            _categoryAreaExporter = categoryAreaExporter;
            _categoryAreaService = categoryAreaService;
        }
        
        [HttpGet]
        public async Task<IEnumerable<CategoryAreaDto>> Get()
        {   
            var categoryAreas = await _categoryAreaService.ReadAllAsync();            
            return categoryAreas.Select(ca => CategoryAreaDto.ConvertFrom(ca)).ToList();
        }
        
        [HttpGet("{id}", Name = "GetCategoryArea")]
        public async Task<IActionResult> Get(long id)
        {
            var categoryArea = await _categoryAreaService.ReadAsync(id);
            var result = CategoryAreaDto.ConvertFrom(categoryArea);
            return new ObjectResult(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CategoryAreaDto categoryAreaDto)
        {
            var categoryArea = new CategoryArea
            {
                Name = categoryAreaDto.Name,
                Description = categoryAreaDto.Description
            };
            await _categoryAreaService.CreateAsync(categoryArea);

            categoryAreaDto.Id = categoryArea.Id;
            return CreatedAtRoute("GetCategoryArea", new { id = categoryAreaDto.Id }, categoryAreaDto);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody]CategoryAreaDto categoryAreaDto)
        {
            if (id != categoryAreaDto.Id)
            {
                throw new ApplicationException($"Invalid Category Area Id:{id}");
            }

            var categoryArea = await _categoryAreaService.ReadAsync(id);
            if (categoryArea == null)
            {
                throw new EntityNotFoundException($"Category Area ({id}) not found.");
            }

            categoryArea.Name = categoryAreaDto.Name;
            categoryArea.Description = categoryAreaDto.Description;
            await _categoryAreaService.UpdateAsync(categoryArea);
            return new NoContentResult();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _categoryAreaService.DeleteAsync(id);
            return new NoContentResult();
        }

        [HttpPost("upload")]
        public async Task<ImportResultDto> Upload()
        {
            var fileContent = await _fileUploadUtility.GetUploadFileContent(Request);
            var importResult = await _categoryAreaImporter.Import(fileContent);
            return new ImportResultDto
            {
                ErrorsCount = importResult.ErrorMessages.Count,
                ErrorMessages = importResult.ErrorMessages.Take(20).ToList(), // send max 20 error messages
            };
        }

        [HttpGet("download")]
        public async Task<IActionResult> Download()
        {
            var allCategoryAreas = await _categoryAreaService.ReadAllAsync();
            var csvContent = _categoryAreaExporter.Export(allCategoryAreas.ToList());            
            var bytes = System.Text.Encoding.Default.GetBytes(csvContent);
            return File(new MemoryStream(bytes), "text/csv", "category-areas.csv");            
        }
    }
}