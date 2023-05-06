using Microsoft.AspNetCore.Mvc;

namespace Francisvac.Result;
public abstract class HttpResponseStrategy
{
    protected HttpResponseStrategy(string message)
        => Message = message;
    public string Message { get; }
    public abstract ActionResult CreateObjectResult();
}

public class OkHttpResponseStrategy : HttpResponseStrategy
{
    public object Data { get; }
    public OkHttpResponseStrategy(object data = null!, string message = "")
        : base(message) { Data = data; }
    public override ActionResult CreateObjectResult()
    {
        if (string.IsNullOrEmpty(Message))
            return new OkObjectResult(Data);
        return new OkObjectResult(new {Message});
    }
}

public class NotFoundHttpResponseStrategy : HttpResponseStrategy
{
    public NotFoundHttpResponseStrategy(string message)
        : base(message){}
    public override ActionResult CreateObjectResult()
        => new NotFoundObjectResult(new {Message});
}

public class BadRequestHttpResponseStrategy : HttpResponseStrategy
{
    public BadRequestHttpResponseStrategy(string message)
        : base(message) {}
    public override ActionResult CreateObjectResult()
        => new BadRequestObjectResult(new { Message });
}