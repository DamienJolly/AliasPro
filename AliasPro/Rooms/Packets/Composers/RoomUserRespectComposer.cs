using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomUserRespectComposer : IMessageComposer
    {
        private readonly IPlayer _player;

        public RoomUserRespectComposer(IPlayer player)
        {
            _player = player;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomUserRespectMessageComposer);
            message.WriteInt((int)_player.Id);
            message.WriteInt(_player.RespectsRecieved);
            return message;
        }
    }
}
