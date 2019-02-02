using System.Collections.Generic;

namespace AliasPro.Navigator.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Models;

    public class UserFlatCatsComposer : IPacketComposer
    {
        private readonly IList<INavigatorCategory> _categories;
        private readonly int _playerRank;

        public UserFlatCatsComposer(IList<INavigatorCategory> categories, int playerRank)
        {
            _categories = categories;
            _playerRank = playerRank;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UserFlatCatsMessageComposer);
            message.WriteInt(_categories.Count);

            foreach (INavigatorCategory category in _categories)
            {
                message.WriteInt(category.Id);
                message.WriteString(category.PublicName);
                message.WriteBoolean(category.MinRank <= _playerRank);
                message.WriteBoolean(false);
                message.WriteString("");
                message.WriteString("");
                message.WriteBoolean(false);
            }
            return message;
        }
    }
}
