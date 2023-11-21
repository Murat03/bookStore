using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete
{
	public class ServiceManager : IServiceManager
	{
		private readonly IBookService _bookService;
		private readonly ICategoryService _categoryService;
		private readonly IAuthenticationService _authenticationService;

		public ServiceManager(IBookService bookService, IAuthenticationService authenticationService, ICategoryService categoryService)
		{
			_bookService = bookService;
			_authenticationService = authenticationService;
			_categoryService = categoryService;
		}

		public IBookService BookService => _bookService;

		public IAuthenticationService AuthenticationService => _authenticationService;

		public ICategoryService CategoryService => _categoryService;
	}
}
