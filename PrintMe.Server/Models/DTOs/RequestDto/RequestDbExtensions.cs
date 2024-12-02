using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Models.DTOs;

public static class RequestDbExtensions
{
    public static RequestDto MapAddPrinterRequestToDto(this AddPrinterRequest request) =>
        new()
        {
            UserId = request.UserId,
            Title = "Adding printer request",
            Description = request.Description,
            UserTextData = string.Join(", ", request.Materials.Select(m => $"{m.PrintMaterialId}:{m.Name}")),
            UserSenderId = request.UserId,
            RequestTypeId = 2,
            LocationX = request.LocationX,
            LocationY = request.LocationY,
            MinModelHeight = request.MinModelHeight,
            MinModelWidth = request.MinModelWidth,
            MaxModelHeight = request.MaxModelHeight,
            MaxModelWidth = request.MaxModelWidth,
            RequestStatusId = 1
        };
    public static RequestDto MapEditPrinterRequestToDto(this EditPrinterRequest request) =>
        new()
        {
            UserId = request.UserId,
            Title = "Editing printer request",
            Description = request.Description,
            UserTextData = string.Join(", ", request.Materials.Select(m => $"{m.PrintMaterialId}:{m.Name}")),
            UserSenderId = request.UserId,
            RequestTypeId = 4,
            RequestStatusId = 1,
            ModelId = request.PrinterID // here ModelId is a printer ID
        };
    public static Request MapDtoToAddRequest(this RequestDto request) =>
        new()
        {
            Description = request.Description,
            UserTextData = request.UserTextData,
            UserSenderId = request.UserSenderId,
            RequestTypeId = request.RequestTypeId,
            ModelId = request.ModelId,
            LocationX = request.LocationX,
            LocationY = request.LocationY,
            MinModelHeight = request.MinModelHeight,
            MinModelWidth = request.MinModelWidth,
            MaxModelHeight = request.MaxModelHeight,
            MaxModelWidth = request.MaxModelWidth,
            RequestStatusId = request.RequestStatusId
        };
    public static Request MapDtoToEditRequest(this RequestDto request) =>
        new()
        {
            UserTextData = request.UserTextData,
            UserSenderId = request.UserSenderId,
            RequestTypeId = request.RequestTypeId,
            ModelId = request.ModelId,
            Description = request.Description,
            RequestStatusId = request.RequestStatusId
        };
}