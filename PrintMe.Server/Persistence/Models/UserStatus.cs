using System;
using System.Collections.Generic;

namespace PrintMe.Server.Persistence.Models;

public partial class UserStatus
{
    public int UserStatusId { get; set; }

    public string Status { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
