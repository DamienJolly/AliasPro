using System.Collections.Generic;
using System.Linq;

namespace AliasPro.Navigator.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Models;
    using Room;
    using Room.Models;

    public class NavigatorSearchResultSetComposer : IPacketComposer
    {
        private readonly string _category;
        private readonly string _data;
        private readonly IList<INavigatorCategory> _categories;
        private readonly IRoomController _roomController;

        public NavigatorSearchResultSetComposer(string category, string data, IList<INavigatorCategory> categories, IRoomController roomController)
        {
            _category = category;
            _data = data;
            _categories = categories;
            _roomController = roomController;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.NavigatorSearchResultSetMessageComposer);
            message.WriteString(_category);
            message.WriteString(_data);

            message.WriteInt(_categories.Count);
            foreach (INavigatorCategory navigatorCategory in _categories)
            {
                IList<IRoom> rooms = navigatorCategory.CategoryType.Search(_roomController, navigatorCategory.Id, _data);
                message.WriteString(navigatorCategory.Identifier);
                message.WriteString(navigatorCategory.PublicName);

                message.WriteInt(1);
                message.WriteBoolean(false);

                message.WriteInt(0);

                if (rooms.Count > 12)
                {
                    rooms = rooms.Take(12).ToList();
                }

                message.WriteInt(rooms.Count);
                foreach (IRoom room in rooms)
                {
                    room.RoomData.Compose(message);
                }
            }
            return message;
        }
    }
}
