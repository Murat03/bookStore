using AutoMapper;
using Entities.DataTransferObjects;
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
		private readonly IRepositoryManager _repositoryManager;
		private readonly IMapper _mapper;

		public BookManager(IRepositoryManager repositorymanager, IMapper mapper)
		{
			_repositoryManager = repositorymanager;
			_mapper = mapper;
		}

		public BookDto CreateOneBook(BookDtoForInsertion bookDto)
		{
			var book = _mapper.Map<Book>(bookDto);
			_repositoryManager.BookRepository.CreateOneBook(book);
			_repositoryManager.SaveChanges();
			return _mapper.Map<BookDto>(book);
		}
		public void UpdateOneBook(int id, BookDtoForUpdate bookDto, bool trackChanges)
		{
			var entity = GetOneBookByIdAndCheckExist(id, trackChanges);
			//mapper

			_mapper.Map(bookDto, entity);

			_repositoryManager.BookRepository.UpdateOneBook(entity);
			_repositoryManager.SaveChanges();
		}
		public void DeleteOneBook(int id, bool trackChanges)
		{
			var book = GetOneBookByIdAndCheckExist(id, trackChanges);
			_repositoryManager.BookRepository.DeleteOneBook(book);
			_repositoryManager.SaveChanges();
		}

		public IEnumerable<BookDto> GetAllBooks(bool trackChanges)
		{
			var books = _repositoryManager.BookRepository.GetAllBooks(trackChanges);
			return _mapper.Map<IEnumerable<BookDto>>(books);
		}

		public Book GetOneBookByIdAndCheckExist(int id, bool trackChanges)
		{
			var book = _repositoryManager.BookRepository.GetOneBook(id, trackChanges);
			if (book is null)
				throw new BookNotFoundException(id);

			return book;
		}
		public BookDto GetOneBookById(int id, bool trackChanges)
		{
			var book = GetOneBookByIdAndCheckExist(id, trackChanges);
			return _mapper.Map<BookDto>(book);
		}

		public (BookDtoForUpdate bookDtoForUpdate, Book book) GetOneBookForPatch(int id, bool trackChanges)
		{
			var entity = GetOneBookByIdAndCheckExist(id, trackChanges);
			var bookDto = _mapper.Map<BookDtoForUpdate>(entity);
			return (bookDto, entity);
		}

		public void SaveChangesForPatch(BookDtoForUpdate bookDtoForUpdate, Book book)
		{
			_mapper.Map(bookDtoForUpdate, book);
			_repositoryManager.SaveChanges();
		}
	}
}
