#nullable enable
using PrintMe.Server.Constants;
using PrintMe.Server.Models.Exceptions;

namespace PrintMe.Server.Models.Filters;

public class RequestFilter
{
    public string? Status
    {
        set
        {
            if (value != null && !DbConstants.RequestStatus.Dictionary.ContainsKey(value))
            {
                throw new NotFoundRequestStatusInDb();
            }
            StatusId = value != null ? DbConstants.RequestStatus.Dictionary.GetValueOrDefault(value) : 0;
        }    
    }

    public string? Type
    {
        set
        {
            if (value != null && !DbConstants.RequestStatus.Dictionary.ContainsKey(value))
            {
                throw new NotFoundRequestTypeInDb();
            }
            TypeId = value != null ? DbConstants.RequestStatus.Dictionary.GetValueOrDefault(value) : 0;
        }
    }

    public int? StatusId;
    public int? TypeId;
}