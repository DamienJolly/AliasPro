using System.Collections.Generic;
using System.Linq;

namespace AliasPro.Navigator.Packets.Outgoing
{
    using Network.Events.Headers;
    using Network.Protocol;
    using Models;
    using Room;
    using Room.Models;

    public class NavigatorSearchResultSetComposer : ServerPacket
    {
        public NavigatorSearchResultSetComposer(string category, string data, IList<INavigatorCategory> categories, IRoomController roomController)
            : base(Outgoing.NavigatorSearchResultSetMessageComposer)
        {
            WriteString(category);
            WriteString(data);

            WriteInt(categories.Count);
            foreach (INavigatorCategory navigatorCategory in categories)
            {
                IList<IRoom> rooms = roomController.GetRoomsByCategorySearch(navigatorCategory.Id, data);
                WriteString(navigatorCategory.Identifier);
                WriteString(navigatorCategory.PublicName);

                WriteInt(1);
                WriteBoolean(false);

                WriteInt(0);

                if (rooms.Count > 12)
                {
                    rooms = rooms.Take(12).ToList();
                }

                WriteInt(rooms.Count);
                foreach(IRoom room in rooms)
                {
                    room.RoomData.Compose(this);
                }
            }
        }
    }
}
