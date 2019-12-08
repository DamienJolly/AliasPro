using AliasPro.API.Network.Events;
using AliasPro.API.Pets.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Pets.Packets.Composers
{
    public class PetBreedsComposer : IPacketComposer
    {
        private readonly string _petName;
        private readonly IPetData _petData;

        public PetBreedsComposer(string petName, IPetData petData)
        {
			_petName = petName;
			_petData = petData;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.PetBreedsMessageComposer);
            message.WriteString(_petName);
			message.WriteInt(_petData.Breeds.Count);
			foreach (IPetBreed breed in _petData.Breeds)
			{
				message.WriteInt(breed.Race);
				message.WriteInt(breed.ColourOne);
				message.WriteInt(breed.ColourTwo);
				message.WriteBoolean(breed.HasColourOne);
				message.WriteBoolean(breed.HasColourTwo);
			}
            return message;
        }
    }
}
