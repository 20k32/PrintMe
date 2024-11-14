using System;
using System.Collections.Generic;

namespace PrintMe.Server.Persistence.Models;

public partial class PrintOrder
{
    public int PrintOrderId { get; set; }

    public int? UserId { get; set; }

    public int? PrinterId { get; set; }

    public decimal Price { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly DueDate { get; set; }

    public string ItemLink { get; set; }

    public int? ItemQuantity { get; set; }

    public string ItemDescription { get; set; }

    public int? ItemMaterialId { get; set; }

    public int? PrintOrderStatusId { get; set; }

    public int? PrintOrderStatusReasonId { get; set; }

    public virtual PrintMaterial1 ItemMaterial { get; set; }

    public virtual PrintOrderStatus PrintOrderStatus { get; set; }

    public virtual PrintOrderStatusReason PrintOrderStatusReason { get; set; }

    public virtual Printer Printer { get; set; }

    public virtual User User { get; set; }
}
