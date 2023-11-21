using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EfCore
{
	public class RepositoryManager : IRepositoryManager
	{
		private readonly IBookRepository _bookRepository;
		private readonly Lazy<ICategoryRepository> _categoryRepository;
		private readonly RepositoryContext _context;
		public RepositoryManager(IBookRepository bookRepository, RepositoryContext context)
		{
			_bookRepository = bookRepository;
			_context = context;
			_categoryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(_context));
		}

		public IBookRepository BookRepository => _bookRepository;

		public ICategoryRepository CategoryRepository => _categoryRepository.Value;

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
