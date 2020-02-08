using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Navigator.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Navigator.Packets.Events
{
    internal class InitializeNavigatorEvent : IMessageEvent
    {
        public short Header => Incoming.InitializeNavigatorMessageEvent;

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            await session.SendPacketAsync(new NavigatorMetaDataParserComposer());
            await session.SendPacketAsync(new NavigatorLiftedRoomsComposer());
            await session.SendPacketAsync(new NavigatorCollapsedCategoriesComposer());
            //todo: saved searches 
            await session.SendPacketAsync(new NavigatorPreferencesComposer(session.Player.PlayerSettings));
        }
    }
}
