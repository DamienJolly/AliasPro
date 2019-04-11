namespace AliasPro.API.Messenger.Models
{
    public interface IMessengerMessage
    {
        uint TargetId { get; }
        string Message { get; }
        int Timestamp { get; }
    }
}
