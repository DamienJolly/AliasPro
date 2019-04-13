using AliasPro.Network.Protocol;
using AliasPro.Players.Types;

namespace AliasPro.API.Messenger.Models
{
    public interface IMessengerFriend
    {
        void Compose(ServerPacket message);

        uint Id { get; }
        string Username { get; set; }
        string Figure { get; set; }
        PlayerGender Gender { get; set; }
        string Motto { get; set; }
        bool InRoom { get; set; }
        bool IsOnline { get; set; }
        int Relation { get; set; }
    }
}
