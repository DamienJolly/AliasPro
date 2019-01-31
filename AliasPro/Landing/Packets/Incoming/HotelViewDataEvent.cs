using System.Threading.Tasks;
using System.Collections.Generic;

namespace AliasPro.Landing.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;
    using Models;

    public class HotelViewDataEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.HotelViewDataMessageEvent;

        private readonly ILandingController _landingController;

        public HotelViewDataEvent(ILandingController landingController)
        {
            _landingController = landingController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            string text = clientPacket.ReadString();
            string name = string.Empty;
            IList<IHallOfFamer> hallOfFamers = await _landingController.GetHallOfFamersAsync();

            string[] splitText = text.Split(',');
            if (splitText.Length >= 2)
            {
                name = splitText[1];
            }

            await session.WriteAndFlushAsync(new HotelViewDataComposer(text, name));
            await session.WriteAndFlushAsync(new HallOfFameComposer(hallOfFamers, name));
        }
    }
}