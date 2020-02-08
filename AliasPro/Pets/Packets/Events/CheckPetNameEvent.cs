using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Pets.Packets.Composers;
using AliasPro.Utilities;
using System.Threading.Tasks;

namespace AliasPro.Pets.Packets.Events
{
    public class CheckPetNameEvent : IMessageEvent
    {
        public short Header => Incoming.CheckPetNameMessageEvent;

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
			string petName = message.ReadString();

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
