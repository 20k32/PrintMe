﻿namespace PrintMe.Server.Persistence.Entities;

public partial class Rating
{
    public int RatingId { get; set; }

    public int? Reviewer { get; set; }

    public int? Target { get; set; }

    public int? Value { get; set; }

    public virtual User ReviewerNavigation { get; set; }

    public virtual User TargetNavigation { get; set; }
}
