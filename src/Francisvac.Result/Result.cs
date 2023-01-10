using System;

namespace Francisvac.Result
{
    public class Result
    {
        /// <summary>
        /// Get the Response linked to Result.
        /// </summary>
        public Response Response { get; protected set; }

        /// <summary>
        /// Get the handler in charge of creating the corresponding ObjectResult.
        /// </summary>
        public HttpHandler HttpHandler { get; protected set; } = null!;

        /// <summary>
        /// Get the current state of the Result.
        /// </summary>
        public bool IsSuccess { get; protected set; }

        public Result() { }

        public Result(Response response) => Response = response;

        public Result(Response response, HttpHandler httpHandler)
        {
            Response = response;
            HttpHandler = httpHandler;
        }

        /// <summary>
        /// Set the handler in charge of creating the ObjectResult.
        /// </summary>
        /// <param name="httpHandler">Handler in charge of creating the corresponding ObjectResult.</param>
        public void SetHandler(HttpHandler httpHandler)
        {
            HttpHandler = httpHandler;
        }

        /// <summary>
        /// Create a Response that implicitly casts to an Error Result.
        /// </summary>
        /// <param name="message">A message that gives information about the reason for the status of the result.</param>
        /// <returns>A error Response</returns>
        public static Response Error(string message)
            => new Response(ResultStatus.Error, message);

        /// <summary>
        /// Create a Response that implicitly casts to an NotFound Result.
        /// </summary>
        /// <param name="message">A message that gives information about the reason for the status of the result.</param>
        /// <returns>A notfound Response</returns>
        public static Response NotFound(string message)
            => new Response(ResultStatus.NotFound, message);

        /// <summary>
        /// Create a Response that implicitly casts to an Success Result.
        /// </summary>
        /// <param name="message">A message that gives information about the reason for the status of the result.</param>
        /// <returns>A success Response</returns>
        public static Response Success(string message)
            => new Response(ResultStatus.Success, message);

        public static implicit operator Result(Response response) =>
                response.ResultStatus switch
                {
                    ResultStatus.Error => new Result(response, new ErrorHandler(response.Message)),
                    ResultStatus.NotFound => new Result(response, new NotFoundHandler(response.Message)),
                    ResultStatus.Success => new Result(response, new SuccessHandler(null!, response.Message)) { IsSuccess = true},
                    _ => throw new InvalidOperationException()
                };
    }

    public class Result<TData> : Result
    {
        /// <summary>
        /// The data that is bound to the Result.
        /// </summary>
        public TData Data { get; set; } = default!;

        public Result() { }

        public Result(TData data, Response response)
            : base(response) => Data = data;

        public Result(TData data, Response response, HttpHandler httpHandler)
            : base(response, httpHandler) => Data = data;

        /// <summary>
        /// Create a Success Result.
        /// </summary>
        /// <param name="data">The data that is bound to the Result.</param>
        /// <param name="message">A message that gives information about the reason for the status of the result.</param>
        /// <returns>A Success Result of <typeparamref name="TData"/></returns>
        public static Result<TData> Success(TData data, string message = "")
            => new Result<TData>(data, new Response(ResultStatus.Success, message), new SuccessHandler(data, message)) { IsSuccess = true };

        public static implicit operator TData(Result<TData> result)
            => result.Data;

        public static implicit operator Result<TData>(TData data)
            => new Result<TData>(data, new Response(ResultStatus.Success, string.Empty), new SuccessHandler(data, string.Empty)) { IsSuccess = true};

        public static implicit operator Result<TData>(Response response) =>
            response.ResultStatus switch
            {
                ResultStatus.Error => new Result<TData>(default!, response, new ErrorHandler(response.Message)),
                ResultStatus.NotFound => new Result<TData>(default!, response, new NotFoundHandler(response.Message)),
                ResultStatus.Success => new Result<TData>(default!, response, new SuccessHandler(null!, response.Message)),
                _ => throw new InvalidOperationException()
            };
    }
}
