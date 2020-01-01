using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using AliasPro.Rooms.Types;

namespace AliasPro.Rooms.Packets.Composers
{
    public class AvatarChatComposer : IPacketComposer
    {
        private readonly int _id;
        private readonly string _message;
        private readonly int _expression;
        private readonly int _colour;
        private readonly RoomChatType _chatType;

        public AvatarChatComposer(int id, string message, int expression, int colour, RoomChatType chatType)
        {
            _id = id;
            _message = message;
            _expression = expression;
            _colour = colour;
            _chatType = chatType;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(GetHeaderId(_chatType));
            message.WriteInt(_id);
            message.WriteString(_message);
            message.WriteInt(_expression);
            message.WriteInt(_colour);
            message.WriteInt(0);
            message.WriteInt(-1);
            return message;
        }

        private short GetHeaderId(RoomChatType chatType)
        {
            switch (chatType)
            {
                case RoomChatType.TALK: default: return Outgoing.AvatarChatMessageComposer;
                case RoomChatType.SHOUT: return Outgoing.AvatarShoutMessageComposer;
                case RoomChatType.WHISPER: return Outgoing.AvatarWhisperMessageComposer;
            }
        }
    }
}
