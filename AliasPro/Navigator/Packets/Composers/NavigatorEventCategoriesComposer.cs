using AliasPro.API.Navigator.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Navigator.Packets.Composers
{
    public class NavigatorEventCategoriesComposer : IMessageComposer
    {
        private readonly ICollection<INavigatorCategory> _categories;

        public NavigatorEventCategoriesComposer(ICollection<INavigatorCategory> categories)
        {
            _categories = categories;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.NavigatorEventCategoriesMessageComposer);
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
