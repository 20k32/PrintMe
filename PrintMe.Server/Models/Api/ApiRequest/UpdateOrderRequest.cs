namespace PrintMe.Server.Models.Api.ApiRequest
{
    public sealed class UpdateFullOrderRequest : INullCheck
    {
        public int OrderId { get; init; }
        public int PrinterId { get; init; }
        public double Price { get; init; }
        public string StartDate { get; init; }
        public string DueDate { get; init; }
        public string ItemLink { get; init; }
        public int ItemQuantity { get; init; }
        public string ItemDescription { get; init; }
        public int ItemMaterialId { get; init; }
        public int PrintOrderStatusId { get; init; }
        public int PrintOrderStatusReasonId { get; init; }
        
        public bool IsNull() => OrderId == default
                                || PrinterId == default
                                || Price == 0d
                                || string.IsNullOrWhiteSpace(StartDate)
                                || string.IsNullOrWhiteSpace(DueDate)
                                || string.IsNullOrWhiteSpace(ItemLink)
                                || ItemQuantity == default
                                || string.IsNullOrWhiteSpace(ItemDescription)
                                || ItemMaterialId == default
                                || PrintOrderStatusId == default
                                || PrintOrderStatusReasonId == default;
    }
}