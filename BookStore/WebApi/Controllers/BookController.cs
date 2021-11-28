using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApi.DBOperations;
using WebApi.BookOperations.GetBooks;
using WebApi.BookOperations.CreateBook;
//using WebApi.BookOperations.CreateBook.CreateBookCommand;


namespace WebApi.AddControllers {
    [ApiController]
    [Route("[controller]s")]
    public class BookController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        public BookController(BookStoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            GetBooksQuery query = new GetBooksQuery(_context);
            var result = query.Handle();
            return Ok(result);
        }


        [HttpGet("{id }")]
        public Book GetById(int id)
        {
            var book = _context.Books.Where(book => book.Id == id).SingleOrDefault();
            return book;
        }

        // [HttpGet]
        // public Book Get([FromQuery] string id)
        // {
        //     var book = BookList.Where(book => book.Id == Convert.ToInt32(id)).SingleOrDefault();
        //     return book;
        // }

        // Post
        [HttpPost]
        public IActionResult AddBook([FromBody] CreateBookModel newBook)
        {
            CreateBookCommand command = new CreateBookCommand(_context);
            command.Model();
            command.Handle();
            var book = _context.Books.SingleOrDefault(x => x.Title == newBook.Title);
            if(book is not null){
                return BadRequest();
            }

            _context.Books.Add(newBook);
            _context.SaveChanges();
            return Ok();
        }

        // Put
        [HttpPut]
         public IActionResult UpdateBook(int id,[FromBody] Book updatedBook)
         {
             var book = _context.Books.SingleOrDefault(x => x.Id == id);
             if(book is null){
                 return BadRequest();
             }
             book.GenreId = updatedBook.GenreId != default ? updatedBook.GenreId : book.GenreId;
             book.PageCount = updatedBook.PageCount != default ? updatedBook.PageCount : book.PageCount;
             book.PublishDate = updatedBook.PublishDate != default ? updatedBook.PublishDate : book.PublishDate;
             book.Title = updatedBook.Title != default ? updatedBook.Title : book.Title;

            _context.SaveChanges();
             return Ok();
         }
        
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _context.Books.SingleOrDefault(x => x.Id == id);
            if(book is null){
                return BadRequest();
            }
            _context.Books.Remove(book);
            _context.SaveChanges();
            return Ok();
        }
    }
}