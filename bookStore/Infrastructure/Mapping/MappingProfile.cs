using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace bookStore.Infrastructure.Mapping
{
	public class MappingProfile : Profile
	{
        public MappingProfile()
        {
            CreateMap<BookDtoForUpdate, Book>().ReverseMap();
            CreateMap<Book, BookDto>();
        }
    }
}
