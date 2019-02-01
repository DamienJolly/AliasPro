using System.Collections.Generic;

namespace AliasPro.Navigator.Packets.Outgoing
{
    using Network.Events.Headers;
    using Network.Protocol;
    using Models;

    public class NavigatorFlatCatsComposer : ServerPacket
    {
        public NavigatorFlatCatsComposer(IList<INavigatorCategory> categories, int playerRank)
            : base(Outgoing.NavigatorFlatCatsMessageComposer)
        {
            WriteInt(categories.Count);

            foreach (INavigatorCategory category in categories)
            {
                WriteInt(category.Id);
                WriteString(category.PublicName);
                WriteBoolean(category.MinRank <= playerRank);
            }
        }
    }
}
