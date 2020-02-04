using AliasPro.API.Chat.Models;
using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModeratorUserChatlogComposer : IMessageComposer
    {
        private readonly IPlayerData _player;
        private readonly IDictionary<IPlayerRoomVisited, ICollection<IChatLog>> _chatLogs;
        
        public ModeratorUserChatlogComposer(
            IPlayerData player, 
            IDictionary<IPlayerRoomVisited, ICollection<IChatLog>> chatLogs)
        {
            _player = player;
            _chatLogs = chatLogs;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.ModeratorUserChatlogMessageComposer);
            message.WriteInt((int)_player.Id);
            message.WriteString(_player.Username);

            message.WriteInt(_chatLogs.Count);
            foreach (KeyValuePair<IPlayerRoomVisited, 
                ICollection<IChatLog>> visit in _chatLogs)
            {
                message.WriteByte(1);
                message.WriteShort(2);
                message.WriteString("roomName");
                message.WriteByte(2);
                message.WriteString(visit.Key.RoomName);
                message.WriteString("roomId");
                message.WriteByte(1);
                message.WriteInt(visit.Key.RoomId);

                message.WriteShort((short)visit.Value.Count);
                foreach (IChatLog chatlog in visit.Value)
                    chatlog.Compose(message);
            }
            return message;
        }
    }
}
