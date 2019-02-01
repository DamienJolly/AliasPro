namespace AliasPro.Room.Models
{
    using Network.Protocol;

    public interface IRoomData
    {
        uint Id { get; set; }
        int Score { get; set; }
        int OwnerId { get; set; }
        string Name { get; set; }
        string Password { get; set; }
        string ModelName { get; set; }
        int UsersNow { get; set; }
        int EnumType { get; }

        void Compose(ServerPacket serverPacket);
    }
}
