using Microsoft.AspNetCore.Http;

namespace Francisvac.Result.AspNetCore;

public static class MinimalApiExtensions
{
    public static Microsoft.AspNetCore.Http.IResult ToMinimalApiResult<TData>(this Result<TData> result)
        => result.Status switch
        {
            ResultStatus.Success => result.Data is null 
                ? Results.Ok(result.Message) 
                : Results.Ok(result.Data),
            ResultStatus.Error => Results.BadRequest(result.Message),
            ResultStatus.NotFound => Results.NotFound(result.Message),
            _ => throw new InvalidResultStatusException()
        };

    public static Microsoft.AspNetCore.Http.IResult ToMinimalApiResult(this Result result)
        => result.Status switch
        {
            ResultStatus.Success => Results.Ok(result.Message),
            ResultStatus.Error => Results.BadRequest(result.Message),
            ResultStatus.NotFound => Results.NotFound(result.Message),
            _ => throw new InvalidResultStatusException()
        };

    public static async Task<Microsoft.AspNetCore.Http.IResult> ToMinimalApiResultAsync<TData>(this Task<Result<TData>> asyncResult)
        => ToMinimalApiResult(await asyncResult);

    public static async Task<Microsoft.AspNetCore.Http.IResult> ToMinimalApiResultAsync<TData>(this Task<Result> asyncResult)
        => ToMinimalApiResult(await asyncResult);
}
