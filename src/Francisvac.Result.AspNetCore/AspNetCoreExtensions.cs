using Microsoft.AspNetCore.Mvc;

namespace Francisvac.Result.AspNetCore;
public static class AspNetCoreExtensions
{
    public static ActionResult ToActionResult<TData>(this Result<TData> result)
    {
        HttpResponseStrategy strategy = result.Status switch
        {
            ResultStatus.Success => new OkHttpResponseStrategy(result.Data!, result.Message),
            ResultStatus.Error => new BadRequestHttpResponseStrategy(result.Message),
            ResultStatus.NotFound => new NotFoundHttpResponseStrategy(result.Message),
            _ => throw new InvalidResultStatusException()
        };
        return strategy.CreateObjectResult();
    }

    public static ActionResult ToActionResult(this Result result)
    {
        HttpResponseStrategy strategy = result.Status switch
        {
            ResultStatus.Success => new OkHttpResponseStrategy(result.Message),
            ResultStatus.Error => new BadRequestHttpResponseStrategy(result.Message),
            ResultStatus.NotFound => new NotFoundHttpResponseStrategy(result.Message),
            _ => throw new InvalidResultStatusException()
        };
        return strategy.CreateObjectResult();
    }

    public static async Task<ActionResult> ToActionResult<TData>(this Task<Result<TData>> taskResult)
    {
        var r = await taskResult;
        HttpResponseStrategy strategy = r.Status switch
        {
            ResultStatus.Success => new OkHttpResponseStrategy(r.Data!, r.Message),
            ResultStatus.Error => new BadRequestHttpResponseStrategy(r.Message),
            ResultStatus.NotFound => new NotFoundHttpResponseStrategy(r.Message),
            _ => throw new InvalidResultStatusException()
        };
        return strategy.CreateObjectResult();
    }

    public static async Task<ActionResult> ToActionResult(this Task<Result> taskResult)
    {
        var r = await taskResult;
        HttpResponseStrategy strategy = r.Status switch
        {
            ResultStatus.Success => new OkHttpResponseStrategy(r.Message),
            ResultStatus.Error => new BadRequestHttpResponseStrategy(r.Message),
            ResultStatus.NotFound => new NotFoundHttpResponseStrategy(r.Message),
            _ => throw new InvalidResultStatusException()
        };
        return strategy.CreateObjectResult();
    }
}