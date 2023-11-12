using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EfCore.Extensions
{
	public static class BookRepositoryExtensions
	{
		public static IQueryable<Book> FilterBooks(this IQueryable<Book> books,
			uint minPrice, uint maxPrice) => 
			books.Where(b => (b.Price >= minPrice) && (b.Price <= maxPrice));

		public static IQueryable<Book> Search(this IQueryable<Book> books,
			string searchTerm)
		{
			if (string.IsNullOrWhiteSpace(searchTerm))
				return books;

			return books.Where(b => b.Title.ToLower().Contains(searchTerm.Trim().ToLower()));
		}
		public static IQueryable<Book> Sort(this IQueryable<Book> books,
			string orderByQueryString)
		{
			if(string.IsNullOrWhiteSpace(orderByQueryString))
				return books.OrderBy(b => b.BookId);

			var orderQuery = OrderQueryBuilder.CreateOrderQuery<Book>(orderByQueryString);

			if(orderQuery is null)
				return books.OrderBy(b => b.BookId);

			return books.OrderBy(orderQuery);
		}
	}
}
