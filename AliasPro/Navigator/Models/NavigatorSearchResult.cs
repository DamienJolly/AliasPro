using AliasPro.API.Navigator.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Protocol;
using System.Collections.Generic;
using System.Linq;

namespace AliasPro.Navigator.Models
{
    internal class NavigatorSearchResult : INavigatorSearchResult
    {
        internal NavigatorSearchResult(int order, string id, string publicName, IList<IRoomData> rooms, bool showMore)
        {
            Order = order;
            Id = id;
            PublicName = publicName;
            Rooms = rooms;
            ShowMore = showMore;
        }

        public void Serialize(ServerPacket message)
        {
            message.WriteString(Id);
            message.WriteString(PublicName);
            message.WriteInt(ShowMore ? 2 : (Rooms.Count > 12 ? 1 : 0)); // Action Allowed (0 (Nothing), 1 (More Results), 2 (Go Back))
            message.WriteBoolean(false); //show collapsed
            message.WriteInt(0); //Display Mode (0 (List), 1 (Thumbnails), 2 (Thumbnail no choice))

            //todo: add take to config?
            var roomsToGo = Rooms.OrderByDescending(i => i.UsersNow).Take(ShowMore ? 50 : 12).ToList();
            message.WriteInt(roomsToGo.Count);
            foreach (IRoomData roomData in roomsToGo)
                roomData.Compose(message);
        }

        public int Order { get; set; }
        public string Id { get; set; }
        public string PublicName { get; set; }
        public IList<IRoomData> Rooms { get; set; }
        public bool ShowMore { get; set; }
    }
}
