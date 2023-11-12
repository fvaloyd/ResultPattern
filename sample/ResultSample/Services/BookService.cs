using Francisvac.Result;

namespace ResultSample.Services;

public record Book(int Id, string title);

public class BookService
{
    private readonly List<Book> _books;

    public BookService()
    {
        _books = new()
        {
            new Book(1, "Book1"),
            new Book(2, "Book2"),
            new Book(3, "Book3"),
            new Book(4, "Book4"),
            new Book(5, "Book5"),
        };
    }

    public Result<IEnumerable<Book>> GetBooks()
        => _books;

    public Result AnyBook(int bookId)
    {
        if (bookId < 0) return Result.Error("Invalid book Id");
        Result result = Result.Error("");
        if (_books.Any(u => u.Id == bookId)) return Result.NotFound($"Not found any book with the id {bookId}");
        return Result.Success("Book exist");
    }

    public Result<Book> GetBook(int bookId)
    {
        if (bookId < 0) return Result.Error("Invalid book Id");
        Book? book = _books.Find(u => u.Id == bookId);
        if (book is null) return Result.NotFound($"Not found any book with the id {bookId}");
        return book;
    }

    public Result AddBook(Book? book)
    {
        if (book is null || book.Id <= 0) return Result.Error("Invalid book");
        if (_books.Any(b => b.Id == book.Id)) return Result.Error("A Book with that id already exist");
        if (book.title.Length < 4) return Result.Error("Invalid title");
        _books.Add(book);
        return Result.Success("Book added successfully");
    }

    public Result DeleteBook(int bookId)
    {
        Book? bookToDelete = _books.Find(u => u.Id == bookId);
        if (bookToDelete is null) return Result.Error("Book not found");
        _books.Remove(bookToDelete);
        return Result.Success("Book delete successfully");
    }
}
