namespace Francisvac.Result;
internal interface IResult
{
    bool IsSuccess { get; }
    string Message { get; }
    ResultStatus Status { get; }
}

internal interface IResult<TData> : IResult
{
    TData Data { get; }
}