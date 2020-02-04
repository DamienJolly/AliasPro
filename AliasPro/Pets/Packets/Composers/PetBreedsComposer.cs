using AliasPro.API.Pets.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Pets.Packets.Composers
{
    public class PetBreedsComposer : IMessageComposer
    {
        private readonly string _petName;
        private readonly IPetData _petData;

        public PetBreedsComposer(string petName, IPetData petData)
        {
			_petName = petName;
			_petData = petData;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.PetBreedsMessageComposer);
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
