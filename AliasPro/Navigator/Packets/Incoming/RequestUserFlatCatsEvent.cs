using System.Threading.Tasks;
using System.Collections.Generic;

namespace AliasPro.Navigator.Packets.Incoming
{
    using Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;

    internal class RequestUserFlatCatsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestUserFlatCatsMessageEvent;

        private readonly INavigatorController _navigatorController;

        public RequestUserFlatCatsEvent(INavigatorController navigatorController)
        {
            _navigatorController = navigatorController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IList<INavigatorCategory> categories = _navigatorController.PromotionCategories;
            await session.SendPacketAsync(new UserFlatCatsComposer(categories, session.Player.Rank));
        }
    }
}
