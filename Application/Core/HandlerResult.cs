namespace Application.Core
{
    // Each handler registered should return a type of HandlerResult
    public class HandlerResult<TResult>
    {
        public bool IsSuccess { get; set; }
        public TResult Value { get; set; }
        public string Error { get; set; }

        // These two static functions provide consistency when
        // representing success and failure objects, brilliant, love it!
        public static HandlerResult<TResult> Success(TResult value) => new HandlerResult<TResult> { IsSuccess = true, Value = value };
        public static HandlerResult<TResult> Failure(string err) => new HandlerResult<TResult> { IsSuccess = false, Error = err };
    }
}