using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.Landing.Models;
using AliasPro.Landing.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Sessions;
using System.Collections.Generic;

namespace AliasPro.Landing.Packets.Events
{
    public class HotelViewDataEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.HotelViewDataMessageEvent;

        private readonly ILandingController _landingController;

        public HotelViewDataEvent(ILandingController landingController)
        {
            _landingController = landingController;
        }

        public async void HandleAsync(
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

            await session.SendPacketAsync(new HotelViewDataComposer(text, name));
            await session.SendPacketAsync(new HallOfFameComposer(hallOfFamers, name));
        }
    }
}