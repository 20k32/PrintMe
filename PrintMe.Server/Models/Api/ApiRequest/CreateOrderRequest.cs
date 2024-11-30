using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace PrintMe.Server.Models.Api.ApiRequest
{
    public sealed class CreateOrderRequest : INullCheck
    {
        public int PrinterId { get; init; }
        public double Price { get; init; }
        public string DueDate { get; init; }
        public string ItemLink { get; init; }
        public int ItemQuantity { get; init; }
        public string ItemDescription { get; init; }
        public int ItemMaterialId { get; init; }

        public bool IsNull() => //UserId == default
                                 PrinterId == default
                                || Price == 0d
                                || string.IsNullOrWhiteSpace(DueDate)
                                || string.IsNullOrWhiteSpace(ItemLink)
                                || ItemQuantity == default
                                || string.IsNullOrWhiteSpace(ItemDescription)
                                || ItemMaterialId == default;
    }
}