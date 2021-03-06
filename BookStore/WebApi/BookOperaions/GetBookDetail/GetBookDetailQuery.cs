using System;
using System.Linq;
using System.Collections.Generic;
using WebApi.Common;
using WebApi.DBOperations;
using AutoMapper;

namespace WebApi.BookOperations.GetBookDetail
{
    public class GetBookDetailQuery
    {
        private readonly BookStoreDbContext _dbContext;
        private readonly IMapper _mapper;
        public int BookId { get; set; }
        public GetBookDetailQuery(BookStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public BookDetailViewModel Handle()
        {
            var book = _dbContext.Books.Where(book => book.Id == BookId).SingleOrDefault();
            if (book is null)
            {
                throw new InvalidOperationException("Book cannot find!");
            }
            // BookDetailViewModel viewModel = new BookDetailViewModel();
            // viewModel.Title = book.Title;
            // viewModel.PageCount = book.PageCount;
            // viewModel.PublishDate = book.PublishDate.Date.ToString("dd/MM/yyyy");
            // viewModel.Genre = ((GenreEnum)book.GenreId).ToString();

            BookDetailViewModel viewModel = _mapper.Map<BookDetailViewModel>(book);

            return viewModel;
        }
    }

    public class BookDetailViewModel
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public int PageCount { get; set; }
        public string PublishDate { get; set; }
    }
}