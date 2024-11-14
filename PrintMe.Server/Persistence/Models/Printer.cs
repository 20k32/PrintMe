﻿using System;
using System.Collections.Generic;

namespace PrintMe.Server.Persistence.Models;

public partial class Printer
{
    public int PrinterId { get; set; }

    public int? PrinterModelId { get; set; }

    public int? UserId { get; set; }

    public string Description { get; set; }

    public double MinModelHeight { get; set; }

    public double MinModelWidth { get; set; }

    public double MaxModelHeight { get; set; }

    public double MaxModelWidth { get; set; }

    public double LocationX { get; set; }

    public double LocationY { get; set; }

    public virtual ICollection<PrintOrder> PrintOrders { get; set; } = new List<PrintOrder>();

    public virtual PrinterModel PrinterModel { get; set; }

    public virtual User User { get; set; }

    public virtual ICollection<PrintMaterial1> Materials { get; set; } = new List<PrintMaterial1>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
