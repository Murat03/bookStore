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
		public async Task<BookDto> CreateOneBookAsync(BookDtoForInsertion bookDto)
		{
			var book = _mapper.Map<Book>(bookDto);
			_repositoryManager.BookRepository.CreateOneBook(book);
			await _repositoryManager.SaveChangesAsync();
			return _mapper.Map<BookDto>(book);
		}
		public async Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDto, bool trackChanges)
		{
			var entity = await GetOneBookByIdAndCheckExistAsync(id, trackChanges);
			//mapper

			_mapper.Map(bookDto, entity);

			_repositoryManager.BookRepository.UpdateOneBook(entity);
			await _repositoryManager.SaveChangesAsync();
		}
		public async Task DeleteOneBookAsync(int id, bool trackChanges)
		{
			var book = await GetOneBookByIdAndCheckExistAsync(id, trackChanges);
			_repositoryManager.BookRepository.DeleteOneBook(book);
			await _repositoryManager.SaveChangesAsync();
		}
		public async Task<IEnumerable<BookDto>> GetAllBooksAsync(bool trackChanges)
		{
			var books = await _repositoryManager.BookRepository.GetAllBooksAsync(trackChanges);
			return _mapper.Map<IEnumerable<BookDto>>(books);
		}
		public async Task<Book> GetOneBookByIdAndCheckExistAsync(int id, bool trackChanges)
		{
			var book = await _repositoryManager.BookRepository.GetOneBookAsync(id, trackChanges);
			if (book is null)
				throw new BookNotFoundException(id);

			return book;
		}
		public async Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges)
		{
			var book = await GetOneBookByIdAndCheckExistAsync(id, trackChanges);
			return _mapper.Map<BookDto>(book);
		}
		public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
		{
			var entity = await GetOneBookByIdAndCheckExistAsync(id, trackChanges);
			var bookDto = _mapper.Map<BookDtoForUpdate>(entity);
			return (bookDto, entity);
		}
		public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
		{
			_mapper.Map(bookDtoForUpdate, book);
			await _repositoryManager.SaveChangesAsync();
		}
	}
}
