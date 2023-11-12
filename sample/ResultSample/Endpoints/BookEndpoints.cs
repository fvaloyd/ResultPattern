using Francisvac.Result.AspNetCore;
using ResultSample.Services;

namespace ResultSample.Endpoints;

public static class BookEndpoints
{
    public static void MapBookEndpoints(this IEndpointRouteBuilder builder)
    {
        var booksGroup = builder.MapGroup("/books").WithOpenApi();

        booksGroup.MapGet("/", (BookService service) => service.GetBooks().ToMinimalApiResult());
        booksGroup.MapGet("/{id}", (BookService service, int id) => service.GetBook(id).ToMinimalApiResult());
        booksGroup.MapPost("/", (BookService service, Book book) => service.AddBook(book).ToMinimalApiResult());
        booksGroup.MapDelete("/{id}", (BookService service, int id) => service.DeleteBook(id).ToMinimalApiResult());
    }
}
