using System;
using System.Collections.Generic;

namespace PrintMe.Server.Persistence.Models;

public partial class RequestStatus
{
    public int RequestStatusId { get; set; }

    public string Status { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
