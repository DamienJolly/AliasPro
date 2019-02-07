﻿using System.Collections.Generic;
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

            List<INavigatorCategory> tempCategories = new List<INavigatorCategory>();
            foreach (INavigatorCategory category in _categories)
            {
                if ((category.Identifier == "popular" && !string.IsNullOrEmpty(_data)) ||
                    (category.Identifier == "query" && string.IsNullOrEmpty(_data)))
                    continue;

                IList<IRoom> rooms = category.CategoryType.Search(_roomController, category.Id, _data);
                if (rooms.Count > 0)
                    tempCategories.Add(category);
            }

            message.WriteInt(tempCategories.Count);
            foreach (INavigatorCategory category in tempCategories)
            {
                IList<IRoom> rooms = category.CategoryType.Search(_roomController, category.Id, _data);
                message.WriteString(category.Identifier);
                message.WriteString(category.PublicName);
                message.WriteInt((rooms.Count > 12) ? 1 : 0);
                message.WriteBoolean(false); //show collapsed
                message.WriteInt(0); //show thumbnails

                if (rooms.Count > 12)
                    rooms = rooms.Take(12).ToList();

                message.WriteInt(rooms.Count);
                foreach (IRoom room in rooms)
                    room.RoomData.Compose(message);
            }
            return message;
        }
    }
}