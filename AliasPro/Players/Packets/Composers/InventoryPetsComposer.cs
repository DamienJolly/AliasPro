using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Players.Packets.Composers
{
    public class InventoryPetsComposer : IPacketComposer
    {
        private readonly ICollection<IPlayerPet> _pets;

        public InventoryPetsComposer(ICollection<IPlayerPet> pets)
        {
			_pets = pets;
		}

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.InventoryPetsMessageComposer);
            message.WriteInt(1); // ??
            message.WriteInt(1); // ??
            message.WriteInt(_pets.Count);
            foreach (IPlayerPet pet in _pets)
            {
				message.WriteInt(pet.Id);
				message.WriteString(pet.Name);
				message.WriteInt(pet.Type);
				message.WriteInt(pet.Race);
				message.WriteString(pet.Colour);
				message.WriteInt(0);
				message.WriteInt(0);
				message.WriteInt(0);
			};
			return message;
        }
    }
}
