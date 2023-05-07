using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Francisvac.Result;
public static class ResultExtensions
{
    /// <summary>
    /// Convert Result of <typeparamref name="T"/> to the corresponding ActionResult.
    /// </summary>
    /// <typeparam name="T">The type that is bound to the Result.</typeparam>
    /// <param name="result">The Result to converted.</param>
    /// <returns>A ActionResult</returns>
    public static ActionResult ToActionResult<T>(this Result<T> result)
        => result.HttpResponseStrategy.CreateObjectResult();

    /// <summary>
    /// Convert the Result to the corresponding ActionResult.
    /// </summary>
    /// <param name="result">The result to be converted.</param>
    /// <returns>A ActionResult</returns>
    public static ActionResult ToActionResult(this Result result)
        => result.HttpResponseStrategy.CreateObjectResult();

    /// <summary>
    /// Convert Result Task to the corresponding ActionResult Task.
    /// </summary>
    /// <param name="taskResult">The Result Task to converted.</param>
    /// <returns>A IActionResult</returns>
    public static async Task<ActionResult> ToActionResult(this Task<Result> taskResult)
    {
        var result = await taskResult;
        return result.HttpResponseStrategy.CreateObjectResult();
    }

    /// <summary>
    /// Convert Result Task of <typeparamref name="T"/> to the corresponding Task of ActionResult.
    /// </summary>
    /// <typeparam name="T">The type that is bound to the Result</typeparam>
    /// <param name="taskResult">The Task of Result to converted.</param>
    /// <returns>A IActionResult</returns>
    public static async Task<ActionResult> ToActionResult<T>(this Task<Result<T>> taskResult)
    {
        var result = await taskResult;
        return result.HttpResponseStrategy.CreateObjectResult();
    }
}