using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Items.Packets.Composers
{
    public class JukeBoxMySongsComposer : IMessageComposer
    {
        private readonly IDictionary<int, int> _items;

        public JukeBoxMySongsComposer(IDictionary<int, int> items)
        {
            _items = items;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.JukeBoxMySongsMessageComposer);
            message.WriteInt(_items.Count);
            foreach (var item in _items)
            {
                message.WriteInt(item.Value);
                message.WriteInt(item.Key);
            }
            return message;
        }
    }
}
