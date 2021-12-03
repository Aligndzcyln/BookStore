using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApi.DBOperations;
using WebApi.BookOperations.GetBooks;
using WebApi.BookOperations.CreateBook;
using static WebApi.BookOperations.CreateBook.CreateBookCommand;
using WebApi.BookOperations.GetBookDetail;
using WebApi.BookOperations.UpdateBook;
using WebApi.BookOperations.DeleteBook;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;

namespace WebApi.AddControllers
{
    [ApiController]
    [Route("[controller]s")]
    public class BookController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        public BookController(BookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            GetBooksQuery query = new GetBooksQuery(_context, _mapper);
            var result = query.Handle();
            return Ok(result);
        }


        [HttpGet("{id }")]
        public IActionResult GetById(int id)
        {
            BookDetailViewModel result;
            try
            {
                GetBookDetailQuery query = new GetBookDetailQuery(_context, _mapper);
                query.BookId = id;
                result = query.Handle();
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }

            var book = _context.Books.Where(book => book.Id == id).SingleOrDefault();
            return Ok(result);
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
            try
            {
                CreateBookCommand command = new CreateBookCommand(_context, _mapper);
                command.Model = newBook;

                CreateBookCommandValidator validator = new CreateBookCommandValidator();
                // ValidationResult result = validator.Validate(command);
                // if (!result.IsValid)
                // {
                //     foreach (var item in result.Errors)
                //     {
                //         Console.WriteLine("Özellik " + item.PropertyName + " Error message: " + item.ErrorMessage);
                //     }
                // }
                // else
                // {
                //     command.Handle();
                // }

                validator.ValidateAndThrow(command);
                command.Handle();


            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


            return Ok();
        }

        // Put
        [HttpPut]
        public IActionResult UpdateBook(int id, [FromBody] UpdateBookModel updatedBook)
        {
            try
            {
                UpdateBookCommand command = new UpdateBookCommand(_context);
                command.BookId = id;
                command.updateBookModel = updatedBook;
                command.Handle();
            }
            catch (System.Exception e)
            {

                return BadRequest(e.Message);
            }

            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            try
            {
                DeleteBookCommand command = new DeleteBookCommand(_context);
                command.BookId = id;
                DeleteBookCommandValidator validator = new DeleteBookCommandValidator();
                validator.ValidateAndThrow(command);
                command.Handle();
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}