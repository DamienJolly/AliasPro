using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomFilterWordsComposer : IMessageComposer
    {
        private readonly ICollection<string> _words;

        public RoomFilterWordsComposer(ICollection<string> words)
        {
            _words = words;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomFilterWordsMessageComposer);
            message.WriteInt(_words.Count);
            foreach (string word in _words)
            {
                message.WriteString(word);
            }
            return message;
        }
    }
}
