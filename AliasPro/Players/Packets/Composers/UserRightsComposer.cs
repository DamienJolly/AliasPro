using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Players.Packets.Composers
{
    public class UserRightsComposer : IMessageComposer
    {
        private readonly IPlayer _player;

        public UserRightsComposer(IPlayer player)
        {
            _player = player;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.UserRightsMessageComposer);
            message.WriteInt(2); //todo: subscription
            message.WriteInt(_player.Rank);
            message.WriteBoolean(false); //todo: ambassador
            return message;
        }
    }
}
