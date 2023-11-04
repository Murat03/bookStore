using Entities.Models;
using Repositories.Contracts;
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

		public IQueryable<Book> GetAllBooks(bool trackChanges) => FindAll(trackChanges);

		public Book GetOneBook(int id, bool trackChanges) => 
			FindByCondition(b => b.BookId.Equals(id), trackChanges).SingleOrDefault();
	}
}
