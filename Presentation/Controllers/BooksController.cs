using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
	[ApiExplorerSettings(GroupName = "v1")]
	[ServiceFilter(typeof(LogFilterAttribute))]
	[ApiController]
	[Route("api/books")]
	//[ResponseCache(CacheProfileName = "5mins")]
	//[HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 80)]
	public class BooksController : ControllerBase
	{
		private readonly IServiceManager _manager;
		public BooksController(IServiceManager manager)
		{
			_manager = manager;
		}

		[Authorize]
		[HttpHead]
		[ServiceFilter(typeof(ValidateMediaTypeAttribute))]
		[HttpGet(Name ="GetAllBooks")]
		//[ResponseCache(Duration = 60)]
		public async Task<IActionResult> GetAllBooks([FromQuery]BookParameters bookParameters)
		{
			var linkParameters = new LinkParameters()
			{
				BookParameters = bookParameters,
				HttpContext = HttpContext
			};

			var result = await _manager.BookService.GetAllBooksAsync(linkParameters, false);

			Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.metaData));

			return result.linkResponse.HasLinks 
				? Ok(result.linkResponse.LinkedEntities)
				: Ok(result.linkResponse.ShapedEntities);
		}
		[Authorize]
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetOneBook([FromRoute(Name = "id")] int id)
		{
			var book = await _manager.BookService.GetOneBookByIdAsync(id, false);
			return Ok(book);
		}
		[Authorize]
		[HttpGet("details")]
		public async Task<IActionResult> GetAllBooksWithDetails()
		{
			var books = await _manager.BookService.GetAllBooksWithDetailsAsync(false);
			return Ok(books);
		}


		[Authorize(Roles = "Editor, Admin")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		[HttpPost(Name ="CreateOneBook")]
		public async Task<IActionResult> CreateOneBook([FromBody] BookDtoForInsertion bookDto)
		{
			var book = await _manager.BookService.CreateOneBookAsync(bookDto);
			return StatusCode(201, book); //CreatedAtRoute()
		}
		[Authorize(Roles = "Editor, Admin")]
		[ServiceFilter(typeof(ValidationFilterAttribute))]
		[HttpPut("{id:int}")]
		public async Task<IActionResult> UpdateOneBook([FromRoute(Name ="id")]int id, [FromBody]BookDtoForUpdate bookDto)
		{
			await _manager.BookService.UpdateOneBookAsync(id, bookDto, false);
			return NoContent();
		}
		[Authorize(Roles = "Admin")]
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteOneBook([FromRoute(Name ="id")]int id)
		{
			await _manager.BookService.DeleteOneBookAsync(id, false);
			return NoContent();
		}
		[Authorize(Roles = "Editor, Admin")]
		[HttpPatch("{id:int}")]
		public async Task<IActionResult> PartiallyUpdateOneBook([FromRoute(Name ="id")]int id, [FromBody]JsonPatchDocument<BookDtoForUpdate> bookPatch)
		{
			if(bookPatch is null)
				return BadRequest();

			var result = await _manager.BookService.GetOneBookForPatchAsync(id, true);
			bookPatch.ApplyTo(result.bookDtoForUpdate, ModelState);

			TryValidateModel(result.bookDtoForUpdate);

			if (!ModelState.IsValid)
				return UnprocessableEntity(ModelState);

			await _manager.BookService.SaveChangesForPatchAsync(result.bookDtoForUpdate, result.book);
			return NoContent();
		}

		[Authorize]
		[HttpOptions]
		public IActionResult GetBooksOptions()
		{
			Response.Headers.Add("Allow", "GET, PUT, POST, PATCH, DELETE, OPTIONS, HEAD");
			return Ok();
		}
	}
}
