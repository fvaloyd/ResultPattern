namespace ResultTests;

public class ResultTests
{
    public record Data(string Some);
    [Fact]
    public void IsSuccess_ShouldBeFalse_WhenErrorResultIsCreated()
    {
        Result result = Result.Error("Test");

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void IsSuccess_ShouldBeFalse_WhenNotFoundResultIsCreated()
    {
        Result result = Result.NotFound("Test");

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void IsSuccess_ShouldBeTrue_WhenSuccessResultIsCreated()
    {
        Result result = Result.Success("Test");

        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public void HttpHandler_ShouldBeErrorHandler_WhenErrorResultIsCreated()
    {
        Result result = Result.Error("Test");

        Assert.Equal(typeof(ErrorHandler), result.HttpHandler.GetType());
    }

    [Fact]
    public void HttpHandler_ShouldBeNotFoundHandler_WhenNotFoundResultIsCreated()
    {
        Result result = Result.NotFound("Test");

        Assert.Equal(typeof(NotFoundHandler), result.HttpHandler.GetType());
    }

    [Fact]
    public void HttpHandler_ShouldBeSuccess_WhenSuccessResultIsCreated()
    {
        Result result = Result.Success("Test");

        Assert.Equal(typeof(SuccessHandler), result.HttpHandler.GetType());
    }

    [Theory]
    [InlineData(ResultStatus.NotFound)]
    [InlineData(ResultStatus.Error)]
    [InlineData(ResultStatus.Success)]
    public void Result_ShouldCastGenericResultFromResponse(ResultStatus status)
    {
        Result result = new Response(status, "");

        Assert.NotNull(result);
        Assert.Equal(status, result.Response.ResultStatus);
    }

    [Fact]
    public void SuccessGeneric_ShouldReturnAResultWithSomeData()
    {
        Result<Data> result = Result<Data>.Success(new Data("Test"));

        Assert.NotNull(result.Data);
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultStatus.Success, result.Response.ResultStatus);
        Assert.Equal(typeof(SuccessHandler), result.HttpHandler.GetType());
    }

    [Fact]
    public void GenericResult_ShouldCastDataFromResult()
    {
        Result<Data> result = Result<Data>.Success(new Data("Test"));

        Data data = result;

        Assert.NotNull(data);
        Assert.Equal("Test", data.Some);
    }
    
    [Fact]
    public void GenericResult_ShouldCastDataResultFromData()
    {
        Data data = new("Test");

        Result<Data> result = data;

        Assert.NotNull(result.Data);
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultStatus.Success, result.Response.ResultStatus);
        Assert.Equal(typeof(SuccessHandler), result.HttpHandler.GetType());
    }
    
    [Theory]
    [InlineData(ResultStatus.NotFound)]
    [InlineData(ResultStatus.Error)]
    [InlineData(ResultStatus.Success)]
    public void GenericResult_ShouldCastGenericResultFromResponse(ResultStatus status)
    {
        Result<Data> result = new Response(status, "");

        Assert.NotNull(result);
        Assert.Null(result.Data);
        Assert.Equal(status, result.Response.ResultStatus);
    }

    [Fact]
    public void ToActionResult_ShouldReturnBadRequestObjectResult_WhenResultIsError()
    {
        Result result = Result.Error("Test error");

        ActionResult actResult = result.ToActionResult();

        Assert.IsType<BadRequestObjectResult>(actResult);
    }
    
    [Fact]
    public void ToActionResult_ShouldReturnNotFoundObjectResult_WhenResultIsNotFound()
    {
        Result result = Result.NotFound("Test notfound");

        ActionResult actResult = result.ToActionResult();

        Assert.IsType<NotFoundObjectResult>(actResult);
    }
    
    [Fact]
    public void ToActionResult_ShouldReturnOkObjectResult_WhenResultIsSuccess()
    {
        Result result = Result.Success("Test success");

        ActionResult actResult = result.ToActionResult();

        Assert.IsType<OkObjectResult>(actResult);
    }
}