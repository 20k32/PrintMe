namespace PrintMe.Server.Models.Api.ApiRequest;

public class CreateChatForMeRequest(int user2Id, bool shouldArchive)
{ 
    public int User2Id { get; init; } = user2Id;
    public bool ShouldArchive { get; init; } = shouldArchive;
    
    public bool IsNull() => User2Id == default;
}