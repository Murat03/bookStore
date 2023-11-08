using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
	[ApiController]
	[Route("api/books")]
	public class BooksController : ControllerBase
	{
		private readonly IServiceManager _manager;
		private readonly IMapper _mapper;
		public BooksController(IServiceManager manager, IMapper mapper)
		{
			_manager = manager;
			_mapper = mapper;
		}
		[HttpGet]
		public async Task<IActionResult> GetAllBooks()
		{
			var books = await _manager.BookService.GetAllBooksAsync(false);
			return Ok(books);
		}
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetOneBook([FromRoute(Name = "id")] int id)
		{
			var book = await _manager.BookService.GetOneBookByIdAsync(id, false);
			return Ok(book);
		}
		[HttpPost]
		public async Task<IActionResult> CreateOneBook([FromBody] BookDtoForInsertion bookDto)
		{
			if (bookDto == null)
				return BadRequest();

			if(!ModelState.IsValid)
				return UnprocessableEntity(ModelState);

			var book = await _manager.BookService.CreateOneBookAsync(bookDto);
			return StatusCode(201, book); //CreatedAtRoute()
		}
		[HttpPut("{id:int}")]
		public async Task<IActionResult> UpdateOneBook([FromRoute(Name ="id")]int id, [FromBody]BookDtoForUpdate bookDto)
		{
			if (bookDto is null)
				return BadRequest();

			if(!ModelState.IsValid)
				return UnprocessableEntity(ModelState);

			await _manager.BookService.UpdateOneBookAsync(id, bookDto, false);
			return NoContent();
		}
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteOneBook([FromRoute(Name ="id")]int id)
		{
			await _manager.BookService.DeleteOneBookAsync(id, false);
			return NoContent();
		}
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
	}
}
