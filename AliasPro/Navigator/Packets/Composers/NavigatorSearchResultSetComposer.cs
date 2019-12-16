using AliasPro.API.Navigator.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Navigator.Packets.Composers
{
    public class NavigatorSearchResultSetComposer : IPacketComposer
    {
        private readonly string _name;
        private readonly string _query;
        private readonly IList<INavigatorSearchResult> _results;

        public NavigatorSearchResultSetComposer(string name, string query, IList<INavigatorSearchResult> results)
        {
            _name = name;
            _query = query;
            _results = results;
        }

        public ServerPacket Compose()
        {
			ServerPacket message = new ServerPacket(Outgoing.NavigatorSearchResultSetMessageComposer);
            message.WriteString(_name);
            message.WriteString(_query);
            message.WriteInt(_results.Count);
            foreach (INavigatorSearchResult result in _results)
            {
                result.Serialize(message);
            }
            return message;
        }
    }
}
