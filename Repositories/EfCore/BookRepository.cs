﻿using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EfCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EfCore
{
	public class BookRepository : RepositoryBase<Book> ,IBookRepository
	{
        public BookRepository(RepositoryContext context):base(context){}

        public void CreateOneBook(Book book) => Create(book);
		public void UpdateOneBook(Book book) => Update(book);
		public void DeleteOneBook(Book book) => Delete(book);

		public async Task<PagedList<Book>> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges)
		{
			var books = await FindAll(trackChanges)
				.FilterBooks(bookParameters.MinPrice, bookParameters.MaxPrice)
				.ToListAsync();

			return PagedList<Book>
				.ToPagedList(books, bookParameters.PageNumber, bookParameters.PageSize);
		}

		public async Task<Book> GetOneBookAsync(int id, bool trackChanges) => 
			await FindByCondition(b => b.BookId.Equals(id), trackChanges).SingleOrDefaultAsync();
	}
}
