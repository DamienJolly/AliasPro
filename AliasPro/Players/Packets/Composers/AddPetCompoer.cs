using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Players.Packets.Composers
{
    public class AddPetCompoer : IMessageComposer
    {
        private readonly IPlayerPet _pet;

        public AddPetCompoer(IPlayerPet pet)
        {
			_pet = pet;
		}

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.AddPetMessageComposer);
			_pet.Serialize(message);
			message.WriteBoolean(false); // ??
			return message;
        }
    }
}
