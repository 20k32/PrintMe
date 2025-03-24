using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Models.Api.ApiRequest;

public sealed class CreateChatRequest(int user1Id, int user2Id, bool shouldArchive) : INullCheck
{
    public int User1Id { get; init; } = user1Id;
    public int User2Id { get; init; } = user2Id;
    public bool ShouldArchive { get; init; } = shouldArchive;


    public bool IsNull() => User1Id == default || User2Id == default;
}