using AliasPro.API.Items;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Players.Packets.Events
{
    public class RequestFurniInventoryEvent : IMessageEvent
    {
        public short Header => Incoming.RequestFurniInventoryMessageEvent;

        private readonly IItemController _itemController;

        public RequestFurniInventoryEvent(IItemController itemController)
        {
            _itemController = itemController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            
            await session.SendPacketAsync(new FurniListComposer(session.Player.Inventory.Items));
        }
    }
}
