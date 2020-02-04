using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class MutedWhisperComposer : IMessageComposer
    {
        private readonly int _seconds;

        public MutedWhisperComposer(int seconds)
        {
            _seconds = seconds;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.MutedWhisperMessageComposer);
            message.WriteInt(_seconds);
            return message;
        }
    }
}
