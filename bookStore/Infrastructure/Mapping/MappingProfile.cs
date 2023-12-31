﻿using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace bookStore.Infrastructure.Mapping
{
	public class MappingProfile : Profile
	{
        public MappingProfile()
        {
            CreateMap<BookDtoForUpdate, Book>().ReverseMap();
            CreateMap<BookDto, BookDtoForUpdate>();
            CreateMap<Book, BookDto>();
            CreateMap<BookDtoForInsertion, Book>();
            CreateMap<UserForRegistrationDto, User>();
        }
    }
}
