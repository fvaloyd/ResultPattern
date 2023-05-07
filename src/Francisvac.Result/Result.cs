namespace Francisvac.Result;

public readonly struct Result : IResult
{
    /// <summary>
    /// The state of the result.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// The status of the result.
    /// </summary>
    public ResultStatus Status { get; }

    /// <summary>
    /// A message that gives information about the reason for the status of the result.
    /// </summary>
    public string Message { get; }

    private Result(string message, ResultStatus status, bool isSuccess)
        => (Message, Status, IsSuccess) = (message, status, isSuccess);

    /// <summary>
    /// Create a Error result.
    /// </summary>
    /// <param name="message">A message that gives information about the reason for the status of the result.</param>
    /// <returns>A error Result</returns>
    public static Result Error(string message)
        => new(message, ResultStatus.Error, false);

    /// <summary>
    /// Create a NotFound Result.
    /// </summary>
    /// <param name="message">A message that gives information about the reason for the status of the result.</param>
    /// <returns>A notfound Result</returns>
    public static Result NotFound(string message)
        => new(message, ResultStatus.NotFound, false);

    /// <summary>
    /// Create a success Result.
    /// </summary>
    /// <param name="message">A message that gives information about the reason for the status of the result.</param>
    /// <returns>A success Result</returns>
    public static Result Success(string message)
        => new(message, ResultStatus.Success, true);

    public static implicit operator Result(ResultStatus status)
        => status switch
        {
            ResultStatus.Error => Error("An error occured."),
            ResultStatus.NotFound => NotFound("Resource not found."),
            ResultStatus.Success => Success("Operation succeded."),
            _ => throw new InvalidResultStatusException()
        };
}

public readonly struct Result<TData> : IResult<TData>
{
    /// <summary>
    /// The data that is bound to the Result.
    /// </summary>
    public TData Data { get; }

    /// <summary>
    /// The state of the result.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// The status of the result.
    /// </summary>
    public ResultStatus Status { get; }

    /// <summary>
    /// A message that gives information about the reason for the status of the result.
    /// </summary>
    public string Message { get; }

    private Result(TData data, string message, ResultStatus status, bool isSuccess)
        => (Data, Message, Status, IsSuccess) = (data, message, status, isSuccess);

    /// <summary>
    /// Create a Success Result of <typeparamref name="TData"/>.
    /// </summary>
    /// <param name="data">The data that is bound to the Result.</param>
    /// <param name="message">A message that gives information about the reason for the status of the result.</param>
    /// <returns>A Success Result of <typeparamref name="TData"/>.</returns>
    public static Result<TData> Success(TData data = default!, string message = "")
        => data switch
        {
            null => new Result<TData>(default!, message, ResultStatus.Success, true),
            _ => new Result<TData>(data, message, ResultStatus.Success, true)
        };

    /// <summary>
    /// Create a Error Result.
    ///</summary>
    /// <param name="message">A message that gives information about the reason for the status of the result.</param>
    /// <returns>A BadRequest Result of <typeparamref name="TData"/>.</returns>
    public static Result<TData> Error(string message)
        => new(default!, message, ResultStatus.Error, false);

    /// <summary>
    /// Create a NotFound Result.
    ///</summary>
    /// <param name="message">A message that gives information about the reason for the status of the result.</param>
    /// <returns>A NotFound Result of <typeparamref name="TData"/>.</returns>
    public static Result<TData> NotFound(string message)
        => new(default!, message, ResultStatus.NotFound, false);

    public static implicit operator TData(Result<TData> result)
        => result.Data;

    public static implicit operator Result<TData>(TData data)
        => data switch
        {
            null => Error("Data is null."),
            _ => Success(data, string.Empty)
        };

    public static implicit operator Result<TData>(Result result)
        => result.Status switch
        {
            ResultStatus.Success => Success(default!, result.Message),
            ResultStatus.Error => Error(result.Message),
            ResultStatus.NotFound => NotFound(result.Message),
            _ => throw new InvalidResultStatusException()
        };
}