using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
	public interface IBookService
	{
		BookDto CreateOneBook(BookDtoForInsertion bookDto);
		void UpdateOneBook(int id, BookDtoForUpdate bookDto, bool trackChanges);
		void DeleteOneBook(int id, bool trackChanges);
		IEnumerable<BookDto> GetAllBooks(bool trackChanges);
		BookDto GetOneBookById(int id, bool trackChanges);
		(BookDtoForUpdate bookDtoForUpdate, Book book) GetOneBookForPatch(int id, bool trackChanges);

		void SaveChangesForPatch(BookDtoForUpdate bookDtoForUpdate, Book book);
	}
}
