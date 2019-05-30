using AliasPro.API.Navigator.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;
using System.Linq;

namespace AliasPro.Navigator.Packets.Composers
{
    public class NavigatorSearchResultSetComposer : IPacketComposer
    {
        private readonly IPlayer _player;
        private readonly string _category;
        private readonly string _data;
        private readonly ICollection<INavigatorCategory> _categories;
        private readonly IRoomController _roomController;

        public NavigatorSearchResultSetComposer(IPlayer player, string category, string data, ICollection<INavigatorCategory> categories, IRoomController roomController)
        {
            _player = player;
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

            List<INavigatorCategory> tempCategories = new List<INavigatorCategory>();
            foreach (INavigatorCategory category in _categories)
            {
                if ((category.Identifier == "popular" && !string.IsNullOrEmpty(_data)) ||
                    (category.Identifier == "query" && string.IsNullOrEmpty(_data)))
                    continue;
                
                ICollection<IRoom> rooms = 
                   category.CategoryType.Search(_roomController, _player, _data);
                if (rooms.Count > 0)
                    tempCategories.Add(category);
            }

            message.WriteInt(tempCategories.Count);
            foreach (INavigatorCategory category in tempCategories)
            {
                ICollection<IRoom> rooms = 
                    category.CategoryType.Search(_roomController, _player, _data);
                message.WriteString(category.Identifier);
                message.WriteString(category.PublicName);
                message.WriteInt((rooms.Count > 12) ? 1 : 0);
                message.WriteBoolean(false); //show collapsed
                message.WriteInt(0); //show thumbnails

                if (rooms.Count > 12)
                    rooms = rooms.Take(12).ToList();

                message.WriteInt(rooms.Count);
                foreach (IRoom room in rooms)
                    room.Compose(message);
            }
            return message;
        }
    }
}
