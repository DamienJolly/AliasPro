using AliasPro.API.Navigator.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Navigator.Packets.Composers
{
    public class NavigatorEventCategoriesComposer : IPacketComposer
    {
        private readonly ICollection<INavigatorCategory> _categories;

        public NavigatorEventCategoriesComposer(ICollection<INavigatorCategory> categories)
        {
            _categories = categories;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.NavigatorEventCategoriesMessageComposer);
            message.WriteInt(_categories.Count);
            foreach (INavigatorCategory category in _categories)
            {
                message.WriteInt(category.SortId);
                message.WriteString(category.PublicName);
                message.WriteBoolean(category.Enabled);
            }
            return message;
        }
    }
}
