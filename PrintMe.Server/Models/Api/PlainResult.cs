using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PrintMe.Server.Models.Api
{ 
    
    public class PlainResult
    {
        public string Message { get; init; }
        public int StatusCode { get; init; }

        public PlainResult(string message, int statusCode)
        {
            Message = message;
            StatusCode = statusCode;
        }

        public ObjectResult ToObjectResult() => new(this) { StatusCode = this.StatusCode };
    }
}