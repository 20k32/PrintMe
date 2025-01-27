using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Models.DTOs.RequestDto
{
    public sealed class RequestStatusDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
    }
}