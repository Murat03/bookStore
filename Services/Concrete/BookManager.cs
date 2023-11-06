using Entities.Exceptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete
{
	public class BookManager : IBookService
	{
		private readonly IRepositoryManager _repositorymanager;

		public BookManager(IRepositoryManager repositorymanager)
		{
			_repositorymanager = repositorymanager;
		}

		public void CreateOneBook(Book book)
		{
			_repositorymanager.BookRepository.CreateOneBook(book);
			_repositorymanager.SaveChanges();
		}
		public void UpdateOneBook(int id, Book book, bool trackChanges)
		{
			var entity = GetOneBookByIdAndCheckExist(id, trackChanges);
			//mapper
			entity.Title = book.Title;
			entity.Price = book.Price;

			_repositorymanager.BookRepository.UpdateOneBook(entity);
			_repositorymanager.SaveChanges();
		}
		public void DeleteOneBook(int id, bool trackChanges)
		{
			var book = GetOneBookByIdAndCheckExist(id, trackChanges);
			_repositorymanager.BookRepository.DeleteOneBook(book);
			_repositorymanager.SaveChanges();
		}

		public IEnumerable<Book> GetAllBooks(bool trackChanges)
		{
			return _repositorymanager.BookRepository.GetAllBooks(trackChanges);
		}

		public Book GetOneBookByIdAndCheckExist(int id, bool trackChanges)
		{
			var book = _repositorymanager.BookRepository.GetOneBook(id, trackChanges);
			if (book is null)
				throw new BookNotFoundException(id);

			return book;
		}

		
	}
}
