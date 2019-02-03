using System.Threading.Tasks;

namespace AliasPro.Item.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;

    public class PlaceItemEvent : IAsyncPacket
    {
        public short Header { get; } = 0;
        
        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            //todo: 
        }
    }
}
