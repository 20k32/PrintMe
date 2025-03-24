using System.Diagnostics;

namespace PrintMe.Server.Models.Api.ApiRequest;

public class ArchiveChatFroMeRequest : INullCheck
{
    public int ChatId { get; init;  }

    public bool IsNull() => ChatId == default;
}

public sealed class ArchiveChatRequest : ArchiveChatFroMeRequest
{
    public int SenderId { get; init; }
    public new bool IsNull() => SenderId == default || ChatId == default;
}