using AliasPro.API.Chat.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModeratorUserChatlogComposer : IPacketComposer
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

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.ModeratorUserChatlogMessageComposer);
            message.WriteInt(_player.Id);
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

                message.WriteShort(visit.Value.Count);
                foreach (IChatLog chatlog in visit.Value)
                    chatlog.Compose(message);
            }
            return message;
        }
    }
}
