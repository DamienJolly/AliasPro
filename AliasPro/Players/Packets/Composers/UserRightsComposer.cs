using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Players.Packets.Composers
{
    public class UserRightsComposer : IPacketComposer
    {
        private readonly IPlayer _player;

        public UserRightsComposer(IPlayer player)
        {
            _player = player;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UserRightsMessageComposer);
            message.WriteInt(2); //todo: subscription
            message.WriteInt(_player.Rank);
            message.WriteBoolean(false); //todo: ambassador
            return message;
        }
    }
}
