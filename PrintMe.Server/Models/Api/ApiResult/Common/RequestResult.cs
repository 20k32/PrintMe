using PrintMe.Server.Models.DTOs;

namespace PrintMe.Server.Models.Api.ApiResult.Common
{
    public class RequestResult : ResultBase
    {
        public IEnumerable<RequestDto> RequestDtos { get; init; }
        
        public RequestResult()
        { }

        public RequestResult(IEnumerable<RequestDto> requestDtos, string message, int statusCode) : base(message, statusCode) 
            => RequestDtos = requestDtos;
    }
}