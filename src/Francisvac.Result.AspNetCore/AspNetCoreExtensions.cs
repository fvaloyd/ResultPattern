using Microsoft.AspNetCore.Mvc;

namespace Francisvac.Result.AspNetCore;
public static class AspNetCoreExtensions
{
    public static ActionResult ToActionResult<TData>(this Result<TData> result)
        => result.Status switch
        {
            ResultStatus.Success => result.Data is null
                ? new OkObjectResult(result.Message)
                : new OkObjectResult(result.Data),
            ResultStatus.Error => new BadRequestObjectResult(result.Message),
            ResultStatus.NotFound => new NotFoundObjectResult(result.Message),
            _ => throw new InvalidResultStatusException()
        };

    public static ActionResult ToActionResult(this Result result)
        => result.Status switch
        {
            ResultStatus.Success => new OkObjectResult(result.Message),
            ResultStatus.Error => new BadRequestObjectResult(result.Message),
            ResultStatus.NotFound => new NotFoundObjectResult(result.Message),
            _ => throw new InvalidResultStatusException()
        };

    public static async Task<ActionResult> ToActionResultAsync<TData>(this Task<Result<TData>> taskResult)
    {
        var r = await taskResult;
        return ToActionResult(r);
    }

    public static async Task<ActionResult> ToActionResultAsync(this Task<Result> taskResult)
    {
        var r = await taskResult;
        return ToActionResult(r);
    }
}
