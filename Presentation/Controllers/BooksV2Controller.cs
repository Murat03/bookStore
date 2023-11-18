﻿using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
	//[ApiVersion("2.0", Deprecated = true)] It is deleted because 
	//Switched to Convention Versioning
	[ApiController]
	[Route("api/books")]
	//[Route("api/{v:apiversion}/books")] //url versioning
	public class BooksV2Controller : ControllerBase
	{
		private readonly IServiceManager _manager;
		public BooksV2Controller(IServiceManager manager)
		{
			_manager = manager;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllBooks()
		{
			var books = await _manager.BookService.GetAllBooksAsync(false);
			var booksV2 = books.Select(b => new
			{
				b.Title,
				b.Id
			});
			return Ok(booksV2);
		}
	}
}