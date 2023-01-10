using Microsoft.AspNetCore.Mvc;

namespace Francisvac.Result
{
    /// <summary>
    /// Handler in charge of creating the corresponding ObjectResult.
    /// </summary>
    public abstract class HttpHandler
    {
        /// <summary>
        /// A message that belongs to the state of the handler that will serve to create the ObjectResult.
        /// </summary>
        protected string _message;

        protected HttpHandler(string message)
        {
            _message = message;
        }

        /// <summary>
        /// Create a ObjectResult.
        /// </summary>
        /// <returns>ObjectResult that matches the status of the Result.</returns>
        public abstract ActionResult CreateObjectResult();
    }

    /// <summary>
    /// Handler in charge of creating NotFoundObjectResult.
    /// </summary>
    public sealed class NotFoundHandler : HttpHandler
    {
        public NotFoundHandler(string message)
            : base(message) { }

        /// <summary>
        /// Create a NotFoundObjectResult.
        /// </summary>
        /// <returns></returns>
        public override ActionResult CreateObjectResult()
        {
            return new NotFoundObjectResult(new { Message = _message });
        }
    }

    /// <summary>
    /// Handler in charge of creating BadRequestObjectResult.
    /// </summary>
    public sealed class ErrorHandler : HttpHandler
    {
        public ErrorHandler(string message)
            : base(message) { }

        /// <summary>
        /// Create a BadRequestObjectResult.
        /// </summary>
        /// <returns></returns>
        public override ActionResult CreateObjectResult()
        {
            return new BadRequestObjectResult(new { Message = _message });
        }
    }

    /// <summary>
    /// Handler in charge of creating OkObjectResult.
    /// </summary>
    public sealed class SuccessHandler : HttpHandler
    {
        /// <summary>
        /// The object that represents the data that will be returned in the request.
        /// </summary>
        private readonly object? _data;

        public SuccessHandler(object? data, string message = "")
            : base(message) { _data = data; }

        /// <summary>
        /// Create a OkObjectResult.
        /// </summary>
        /// <returns></returns>
        public override ActionResult CreateObjectResult()
        {
            if (_data is null)
            {
                return new OkObjectResult(new { Message = _message });
            }
            return new OkObjectResult(_data);
        }
    }
}
