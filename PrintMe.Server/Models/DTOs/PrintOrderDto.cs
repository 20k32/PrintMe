namespace PrintMe.Server.Models.DTOs
{
    public class PrintOrderDto : INullCheck
    {
        public int UserId { get; init; }
        public int PrinterId { get; init; }
        public double Price { get; init; }
        public DateOnly StartDate { get; init; }
        public DateOnly DueDate { get; init; }
        public string ItemLink { get; init; }
        public int ItemQuantity { get; init; }
        public string ItemDescription { get; init; }
        public int ItemMaterialId { get; init; }
        public int PrintOrderStatusId { get; init; }
        public int PrintOrderStatusReasonId { get; init; }

        public bool IsNull() => UserId == default
                                || PrinterId == default
                                || Price == 0d
                                || StartDate == default
                                || DueDate == default
                                || string.IsNullOrWhiteSpace(ItemLink)
                                || ItemQuantity == default
                                || string.IsNullOrWhiteSpace(ItemDescription)
                                || ItemMaterialId == default
                                || PrintOrderStatusId == default
                                || PrintOrderStatusReasonId == default;
    }
}