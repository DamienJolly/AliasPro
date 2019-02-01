using System.Collections.Generic;

namespace AliasPro.Navigator.Packets.Outgoing
{
    using Network.Events.Headers;
    using Network.Protocol;
    using Models;

    public class UserFlatCatsComposer : ServerPacket
    {
        public UserFlatCatsComposer(IList<INavigatorCategory> categories, int playerRank)
            : base(Outgoing.UserFlatCatsMessageComposer)
        {
            WriteInt(categories.Count);

            foreach (INavigatorCategory category in categories)
            {
                WriteInt(category.Id);
                WriteString(category.PublicName);
                WriteBoolean(category.MinRank <= playerRank);
                WriteBoolean(false);
                WriteString("");
                WriteString("");
                WriteBoolean(false);
            }
        }
    }
}
