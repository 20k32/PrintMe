using System.Text.Json.Serialization;

namespace PrintMe.Server.Models.Api
{
    public class ApiResult<T> : PlainResult
    {
        public T Value;

        public ApiResult(T value, string message, int statusCode) : base(message, statusCode) =>
            (Value) = (value);
    }
}