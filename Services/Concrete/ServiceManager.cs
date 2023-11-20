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
		private readonly IAuthenticationService _authenticationService;

		public ServiceManager(IBookService bookService, IAuthenticationService authenticationService)
		{
			_bookService = bookService;
			_authenticationService = authenticationService;
		}

		public IBookService BookService => _bookService;

		public IAuthenticationService AuthenticationService => _authenticationService;
	}
}
