using System;
using System.Collections.Generic;

namespace PrintMe.Server.Persistence.Models;

public partial class Request
{
    public int RequestId { get; set; }

    public string UserTextData { get; set; }

    public int UserSenderId { get; set; }

    public int RequestTypeId { get; set; }

    public int? ReportedUserId { get; set; }

    public int? DeleteUserId { get; set; }

    public int? ModelId { get; set; }

    public string Description { get; set; }

    public double? LocationX { get; set; }

    public double? LocationY { get; set; }

    public double? MinModelHeight { get; set; }

    public double? MinModelWidth { get; set; }

    public double? MaxModelHeight { get; set; }

    public double? MaxModelWidth { get; set; }

    public int RequestStatusId { get; set; }

    public int? RequestStatusReasonId { get; set; }

    public virtual User DeleteUser { get; set; }

    public virtual User ReportedUser { get; set; }

    public virtual RequestStatus RequestStatus { get; set; }

    public virtual RequestStatusReason RequestStatusReason { get; set; }

    public virtual RequestType RequestType { get; set; }

    public virtual User UserSender { get; set; }

    public virtual ICollection<PrintMaterial> PrintMaterials { get; set; } = new List<PrintMaterial>();
}
