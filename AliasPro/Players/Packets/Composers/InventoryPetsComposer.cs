using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Players.Packets.Composers
{
    public class InventoryPetsComposer : IMessageComposer
    {
        private readonly ICollection<IPlayerPet> _pets;

        public InventoryPetsComposer(ICollection<IPlayerPet> pets)
        {
			_pets = pets;
		}

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.InventoryPetsMessageComposer);
            message.WriteInt(1); // ??
            message.WriteInt(1); // ??

            message.WriteInt(_pets.Count);
            foreach (IPlayerPet pet in _pets)
            {
				pet.Serialize(message);
			}

			return message;
        }
    }
}
