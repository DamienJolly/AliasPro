using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Pets.Packets.Composers;
using AliasPro.Utilities;

namespace AliasPro.Pets.Packets.Events
{
    public class CheckPetNameEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.CheckPetNameMessageEvent;

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
			string petName = clientPacket.ReadString();

			int minLength = 2; //todo: add to config
			int maxLength = 15;

			if (petName.Length < minLength)
			{
				await session.SendPacketAsync(new PetNameErrorComposer(PetNameErrorComposer.NAME_TO_SHORT, minLength.ToString()));
				return;
			}

			if (petName.Length > maxLength)
			{
				await session.SendPacketAsync(new PetNameErrorComposer(PetNameErrorComposer.NAME_TO_LONG, maxLength.ToString()));
				return;
			}

			if (!StringUtils.IsAlphanumeric(petName))
			{
				await session.SendPacketAsync(new PetNameErrorComposer(PetNameErrorComposer.FORBIDDEN_CHAR, petName));
				return;
			}
		
   
			await session.SendPacketAsync(new PetNameErrorComposer(PetNameErrorComposer.NAME_OK, petName));
        }
    }
}
