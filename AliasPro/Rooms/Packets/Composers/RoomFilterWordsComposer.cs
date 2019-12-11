using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomFilterWordsComposer : IPacketComposer
    {
        private readonly ICollection<string> _words;

        public RoomFilterWordsComposer(ICollection<string> words)
        {
            _words = words;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomFilterWordsMessageComposer);
            message.WriteInt(_words.Count);
            foreach (string word in _words)
            {
                message.WriteString(word);
            }
            return message;
        }
    }
}
