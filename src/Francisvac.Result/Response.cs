namespace Francisvac.Result
{
    /// <summary>
    /// Response linked to Result.
    /// </summary>
    public struct Response
    {
        /// <summary>
        /// Result Status.
        /// </summary>
        public ResultStatus ResultStatus { get; set; }

        /// <summary>
        /// Message to inform about the status of the Result.
        /// </summary>
        public string Message { get; set; }
        
        public Response(ResultStatus resultStatus, string message)
        {
            ResultStatus = resultStatus;
            Message = message;
        }

        /// <summary>
        /// Create a Error Response.
        /// </summary>
        /// <param name="message">A message that gives information about the reason for the status of the Result.</param>
        /// <returns>Error Response.</returns>
        public static Response Error(string message)
            => new Response(ResultStatus.Error, message);

        /// <summary>
        /// Create NotFound Response.
        /// </summary>
        /// <param name="message">A message that gives information about the reason for the status of the result.</param>
        /// <returns>NotFound Response.</returns>
        public static Response NotFound(string message)
            => new Response(ResultStatus.NotFound, message);

        /// <summary>
        /// Create Success Response
        /// </summary>
        /// <param name="message">A message that gives information about the reason for the status of the result.</param>
        /// <returns>Success Response.</returns>
        public static Response Success(string message)
            => new Response(ResultStatus.Success, message);

        /// <summary>
        /// Create Success Response, typically you will use this version when you are not interested in the message.
        /// </summary>
        /// <returns>Success Response with empty message.</returns>
        public static Response Success()
            => new Response(ResultStatus.Success, string.Empty);
    }
}
