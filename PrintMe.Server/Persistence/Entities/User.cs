namespace PrintMe.Server.Persistence.Entities;

public partial class User
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public int? UserStatusId { get; set; }
    public bool? ShouldHidePhoneNumber { get; set; }
    public string Description { get; set; }
    public string Password { get; set; }
    public string PasswordSalt { get; set; }
    public int UserRoleId { get; set; }
    public string ConfirmationToken { get; set; }
    public bool IsVerified { get; set; }
    public virtual ICollection<Chat> ChatUser1s { get; set; } = new List<Chat>();

    public virtual ICollection<Chat> ChatUser2s { get; set; } = new List<Chat>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<PrintOrder> PrintOrders { get; set; } = new List<PrintOrder>();

    public virtual ICollection<Printer> PrintersNavigation { get; set; } = new List<Printer>();

    public virtual ICollection<Rating> RatingReviewerNavigations { get; set; } = new List<Rating>();

    public virtual ICollection<Rating> RatingTargetNavigations { get; set; } = new List<Rating>();

    public virtual ICollection<Request> RequestDeleteUsers { get; set; } = new List<Request>();

    public virtual ICollection<Request> RequestReportedUsers { get; set; } = new List<Request>();

    public virtual ICollection<Request> RequestUserSenders { get; set; } = new List<Request>();

    public virtual UserStatus UserStatus { get; set; }
    
    public virtual UserRole UserRole { get; set; }
    public virtual ICollection<Printer> Printers { get; set; } = new List<Printer>();
}
