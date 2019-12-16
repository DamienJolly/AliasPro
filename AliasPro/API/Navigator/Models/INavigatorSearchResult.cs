using AliasPro.API.Rooms.Models;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.API.Navigator.Models
{
    public interface INavigatorSearchResult
    {
        void Serialize(ServerPacket message);

        int Order { get; set; }
        string Id { get; set; }
        string PublicName { get; set; }
        IList<IRoomData> Rooms { get; set; }
        bool ShowMore { get; set; }
    }
}
