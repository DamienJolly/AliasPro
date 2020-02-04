using AliasPro.API.Navigator.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Navigator.Packets.Composers
{
    public class UserFlatCatsComposer : IMessageComposer
    {
        private readonly ICollection<INavigatorCategory> _categories;
        private readonly int _playerRank;

        public UserFlatCatsComposer(ICollection<INavigatorCategory> categories, int playerRank)
        {
            _categories = categories;
            _playerRank = playerRank;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.UserFlatCatsMessageComposer);
            message.WriteInt(_categories.Count);

            foreach (INavigatorCategory category in _categories)
            {
                message.WriteInt(category.SortId);
                message.WriteString(category.PublicName);
                message.WriteBoolean(category.MinRank <= _playerRank);
                message.WriteBoolean(!category.Enabled);
                message.WriteString(""); //??
                message.WriteString(""); //??
                message.WriteBoolean(false);
            }
            return message;
        }
    }
}
