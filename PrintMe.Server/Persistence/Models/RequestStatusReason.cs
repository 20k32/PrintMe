using System;
using System.Collections.Generic;

namespace PrintMe.Server.Persistence.Models;

public partial class RequestStatusReason
{
    public int RequestStatusReasonId { get; set; }

    public string Reason { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
