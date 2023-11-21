using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.LinkModels;
using Entities.Models;
using Entities.RequestFeatures;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete
{
	public class BookManager : IBookService
	{
		private readonly ICategoryService _categoryService;
		private readonly IRepositoryManager _repositoryManager;
		private readonly IMapper _mapper;
		private readonly IDataShaper<BookDto> _shaper;
		private readonly IBookLinks _bookLinks;

		public BookManager(IRepositoryManager repositorymanager, IMapper mapper, IDataShaper<BookDto> shaper, IBookLinks bookLinks, ICategoryService categoryService)
		{
			_repositoryManager = repositorymanager;
			_mapper = mapper;
			_shaper = shaper;
			_bookLinks = bookLinks;
			_categoryService = categoryService;
		}
		public async Task<BookDto> CreateOneBookAsync(BookDtoForInsertion bookDto)
		{
			var category = await _categoryService.GetOneCategoryByIdAsync(bookDto.CategoryId, false);

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
		public async Task<(LinkResponse linkResponse, MetaData metaData)> GetAllBooksAsync(LinkParameters linkParameters, bool trackChanges)
		{
			if(!linkParameters.BookParameters.ValidPriceRange)
				throw new PriceOutOfRangeBadRequestException();

			var booksWithMetaData = await _repositoryManager.BookRepository.GetAllBooksAsync(linkParameters.BookParameters,trackChanges);
			var booksDto = _mapper.Map<IEnumerable<BookDto>>(booksWithMetaData);

			var links = _bookLinks.TryGenerateLinks(booksDto,
				linkParameters.BookParameters.Fields,
				linkParameters.HttpContext);

			return (linkResponse: links, metaData: booksWithMetaData.MetaData);
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

		public async Task<List<Book>> GetAllBooksAsync(bool trackChanges)
		{
			return await _repositoryManager.BookRepository.GetAllBooksAsync(trackChanges);
		}

		public async Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync(bool trackChanges)
		{
			return await _repositoryManager.BookRepository.GetAllBooksWithDetailsAsync(trackChanges);
		}
	}
}
