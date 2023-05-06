using System;

namespace Francisvac.Result;
public class InvalidResultStatusException : Exception
{
    public InvalidResultStatusException() : base() {}
    public InvalidResultStatusException(string? message) : base(message) {}
    public InvalidResultStatusException(string? message, Exception? innerException) : base(message, innerException) {}
}