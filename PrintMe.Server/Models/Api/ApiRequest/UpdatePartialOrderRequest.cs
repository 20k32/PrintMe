namespace PrintMe.Server.Models.Api.ApiRequest;

public class UpdatePartialOrderRequest
{
	public int OrderId { get; init; }
	public double Price { get; init; }
	public string DueDate { get; init; }
	public string ItemLink { get; init; }
	public int ItemQuantity { get; init; }
	public string ItemDescription { get; init; }
	public int ItemMaterialId { get; init; }

	public bool IsNull() => OrderId == default
							|| Price == 0d
							|| DueDate == default
							|| string.IsNullOrWhiteSpace(ItemLink)
							|| ItemQuantity == default
							|| string.IsNullOrWhiteSpace(ItemDescription)
							|| ItemMaterialId == default;
}