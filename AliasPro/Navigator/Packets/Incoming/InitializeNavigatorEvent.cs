using System.Threading.Tasks;

namespace AliasPro.Navigator.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;

    internal class InitializeNavigatorEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.InitializeNavigatorMessageEvent;

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            await session.SendPacketAsync(new NavigatorMetaDataParserComposer());
            await session.SendPacketAsync(new NavigatorLiftedRoomsComposer());
            await session.SendPacketAsync(new NavigatorCollapsedCategoriesComposer());
            await session.SendPacketAsync(new NavigatorPreferencesComposer());
        }
    }
}
