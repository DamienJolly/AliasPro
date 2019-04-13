using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Navigator.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Navigator.Packets.Events
{
    internal class InitializeNavigatorEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.InitializeNavigatorMessageEvent;

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            await session.SendPacketAsync(new NavigatorMetaDataParserComposer());
            await session.SendPacketAsync(new NavigatorLiftedRoomsComposer());
            await session.SendPacketAsync(new NavigatorCollapsedCategoriesComposer());
            await session.SendPacketAsync(new NavigatorPreferencesComposer(session.Player.PlayerSettings));
        }
    }
}
