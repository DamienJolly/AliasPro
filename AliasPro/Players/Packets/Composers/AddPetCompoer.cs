using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Players.Packets.Composers
{
    public class AddPetCompoer : IPacketComposer
    {
        private readonly IPlayerPet _pet;

        public AddPetCompoer(IPlayerPet pet)
        {
			_pet = pet;
		}

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.AddPetMessageComposer);
			_pet.Serialize(message);
			message.WriteBoolean(false); // ??
			return message;
        }
    }
}
