namespace PrintMe.Server.Models.Api.ApiRequest
{
    public class UpdateOrderStatusRequest
    {
        public int OrderId { get; init; }
        public int PrintOrderStatusId { get; init; }
        public int PrintOrderStatusReasonId { get; init; } 
    }
}