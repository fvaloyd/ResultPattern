using Francisvac.Result.AspNetCore;

namespace ResultTests;
public class ResultTests
{
    record Data(string Some);
    readonly Data DATA = new("Test");

    [Fact]
    public void IsSuccess_ShouldBeFalse_WhenErrorResultIsCreated()
    {
        Result result = Result.Error("Test");
        Result<int> resultGeneric = Result<int>.Error("");
        Assert.False(result.IsSuccess);
        Assert.False(resultGeneric.IsSuccess);
    }

    [Fact]
    public void IsSuccess_ShouldBeFalse_WhenNotFoundResultIsCreated()
    {
        Result result = Result.NotFound("Test");
        Result<int> resultGeneric = Result<int>.NotFound("");
        Assert.False(result.IsSuccess);
        Assert.False(resultGeneric.IsSuccess);
    }

    [Fact]
    public void IsSuccess_ShouldBeTrue_WhenSuccessResultIsCreated()
    {
        Result result = Result.Success("Test");
        Result<int> resultGeneric = Result<int>.Success(11, "");
        Assert.True(result.IsSuccess);
        Assert.True(resultGeneric.IsSuccess);
    }

    [Fact]
    public void ResultGeneric_WithSuccessStatus_ShouldReturnAResultWithSomeData()
    {
        Result<Data> result = Result<Data>.Success(DATA, string.Empty);
        Assert.NotNull(result.Data);
        Assert.Equal(DATA, result.Data);
    }

    [Fact]
    public void ResultGeneric_ShouldCastDataFromResult()
    {
        Data rData = Result<Data>.Success(DATA, "");
        Assert.NotNull(rData);
        Assert.Equal(DATA, rData);
    }

    [Fact]
    public void ResultGeneric_ShouldCastSuccessResultFromData()
    {
        Result<Data> result = DATA;
        Assert.NotNull(result.Data);
        Assert.True(result.IsSuccess);
        Assert.Equal(DATA, result.Data);
        Assert.Equal(ResultStatus.Success, result.Status);
    }

    [Fact]
    public void ToActionResult_ShouldReturnBadRequestObjectResult_WhenResultIsError()
    {
        Result result = Result.Error("Test");
        Result<Data> resultGeneric = Result<Data>.Error("");
        ActionResult actResult = result.ToActionResult();
        ActionResult actResultGeneric = resultGeneric.ToActionResult();
        Assert.IsType<BadRequestObjectResult>(actResult);
        Assert.IsType<BadRequestObjectResult>(actResultGeneric);
    }

    [Fact]
    public void ToActionResult_ShouldReturnNotFoundObjectResult_WhenResultIsNotFound()
    {
        Result result = Result.NotFound("Test");
        Result<Data> resultGeneric = Result<Data>.NotFound("Test");
        ActionResult actResult = result.ToActionResult();
        ActionResult actResultGeneric = resultGeneric.ToActionResult();
        Assert.IsType<NotFoundObjectResult>(actResult);
        Assert.IsType<NotFoundObjectResult>(actResultGeneric);
    }

    [Fact]
    public void ToActionResult_ShouldReturnOkObjectResult_WhenResultIsSuccess()
    {
        Result result = Result.Success("Test");
        Result<Data> resultGeneric = Result<Data>.Success(DATA, "Test");
        ActionResult actResult = result.ToActionResult();
        ActionResult actResultGeneric = resultGeneric.ToActionResult();
        Assert.IsType<OkObjectResult>(actResult);
        Assert.IsType<OkObjectResult>(actResultGeneric);
    }
}
