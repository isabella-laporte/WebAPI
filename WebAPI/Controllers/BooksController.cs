using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WebAPI.DTO;
using WebAPI.Filters;
using WebAPI.Interfaces;
using WebAPI.Models;
using WebAPI.Repositories;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController, Authorize]

    public class BooksController : ControllerBase, IBaseController<Books, BooksDTO, BooksPatchRatingDTO>
    {
        private readonly IBaseRepository<Books> _repository;

        public BooksController(IBaseRepository<Books> repository)
        {
            _repository = repository;
        }
        private Books UpdateBooksModel(Books newData, BooksDTO entity)
        {
            newData.BookName = entity.BookName;
            newData.Author = entity.Author;
            newData.UserRating = entity.UserRating;
            newData.Year = entity.Year;
            newData.Genres = entity.Genres;
            return newData;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Books), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromQuery] int page, int maxResult)
        {
            var books = await _repository.Get(page, maxResult);
            if (books == null)
            { return NotFound("Data not found");
            }
            return Ok(books);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Books), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var book = await _repository.GetByKey(id);
            if (book == null)
            {
                return NotFound("Id not found");
            }
            return Ok(book);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Books), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Books), StatusCodes.Status201Created)]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] BooksDTO entity)
        {
            var databaseBooks = await _repository.GetByKey(id);

            if (databaseBooks == null)
            {
                var bookInsert = new Books(id: 0, entity.BookName, entity.Author, entity.UserRating, entity.Year, entity.Genres);
                var inserted = await _repository.Insert(bookInsert);
                return Created(string.Empty, inserted);
            }

            databaseBooks = UpdateBooksModel(databaseBooks, entity);

            var updated = await _repository.Update(databaseBooks);

            return Ok(updated);
        }


        [HttpPost]
        [ProducesResponseType(typeof(Books), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Books), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] BooksDTO entity)
        {
            var booksInserted = new Books(id: 0, entity.BookName, entity.Author, entity.UserRating, entity.Year, entity.Genres);
            var inserted = await _repository.Insert(booksInserted);
            return Created(string.Empty, inserted);
        }


        [HttpPost("query")]
        [ProducesResponseType(typeof(Books), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Books), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post2([FromBody] BooksDTO entity)
        {
            var book = await _repository.Get(1, 5);
            var filtered = book.Where(g => g.BookName.Contains(entity.BookName) &&
            g.Author.Contains(entity.Author) &&
            g.Genres.Contains(entity.Genres)).ToList();

            if (filtered.Count < 1)
                throw new KeyNotFoundException();

            return Ok(filtered);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(Books), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Books), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Patch([FromRoute] int id, [FromBody] BooksPatchRatingDTO entity)
        {
            var databasebooks = await _repository.GetByKey(id);

            if (databasebooks == null)
            {
                return NoContent();
            }

            databasebooks.UserRating = entity.UserRating;

            var update = await _repository.Update(databasebooks);
            return Ok(update);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Books), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Books), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var databasebooks = await _repository.GetByKey(id);

            if (databasebooks == null)
            {
                return NoContent();
            }

            var deleted = await _repository.Delete(id);
            return Ok(deleted);
        }
    }
}
