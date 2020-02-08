using AliasPro.API.Landing;
using AliasPro.API.Landing.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Landing.Packets.Composers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Landing.Packets.Events
{
    public class HotelViewDataEvent : IMessageEvent
    {
        public short Header => Incoming.HotelViewDataMessageEvent;

        private readonly ILandingController _landingController;

        public HotelViewDataEvent(ILandingController landingController)
        {
            _landingController = landingController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
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