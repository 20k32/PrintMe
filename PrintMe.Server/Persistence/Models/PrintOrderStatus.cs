using System;
using System.Collections.Generic;

namespace PrintMe.Server.Persistence.Models;

public partial class PrintOrderStatus
{
    public int PrintOrderStatusId { get; set; }

    public string Status { get; set; }

    public virtual ICollection<PrintOrder> PrintOrders { get; set; } = new List<PrintOrder>();
}
