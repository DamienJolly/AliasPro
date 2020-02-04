using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Types;

namespace AliasPro.Rooms.Packets.Composers
{
    public class AvatarChatComposer : IMessageComposer
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

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(GetHeaderId(_chatType));
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
