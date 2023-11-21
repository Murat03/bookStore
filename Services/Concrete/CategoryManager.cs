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
	public class CategoryManager : ICategoryService
	{
		private readonly IRepositoryManager _repositoryManager;

		public CategoryManager(IRepositoryManager repositoryManager)
		{
			_repositoryManager = repositoryManager;
		}

		public async Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges)
		{
			return await _repositoryManager.CategoryRepository.GetAllCategoriesAsync(trackChanges);
		}

		public async Task<Category> GetOneCategoryByIdAsync(int id, bool trackChanges)
		{
			var category = await _repositoryManager.CategoryRepository.GetOneCategoryByIdAsync(id, trackChanges);
			if (category == null)
				throw new CategoryNotFoundException(id);
			return category;
		}
	}
}
