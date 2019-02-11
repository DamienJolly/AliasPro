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

    internal class RequestNavigatorFlatsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestNavigatorFlatsMessageEvent;

        private readonly INavigatorController _navigatorController;

        public RequestNavigatorFlatsEvent(INavigatorController navigatorController)
        {
            _navigatorController = navigatorController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            if (!_navigatorController.TryGetCategories("roomads_view", out ICollection<INavigatorCategory> categories)) return;
            
            await session.SendPacketAsync(new NavigatorFlatCatsComposer(categories, session.Player.Rank));
        }
    }
}
