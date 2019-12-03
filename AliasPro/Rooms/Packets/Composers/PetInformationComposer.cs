using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using AliasPro.Rooms.Entities;

namespace AliasPro.Rooms.Packets.Composers
{
    public class PetInformationComposer : IPacketComposer
    {
        private readonly PetEntity _petEntity;

        public PetInformationComposer(PetEntity petEntity)
        {
			_petEntity = petEntity;
        }

        public ServerPacket Compose()
        {
			ServerPacket message = new ServerPacket(Outgoing.PetInformationMessageComposer);
			message.WriteInt(_petEntity.Id);
			message.WriteString(_petEntity.Name);
			message.WriteInt(0); //level
			message.WriteInt(20); //max level
			message.WriteInt(0); //exp
			message.WriteInt(20); //xp goal
			message.WriteInt(80); //energy
			message.WriteInt(100); //max energy
			message.WriteInt(70); //happyness
			message.WriteInt(100); //max happyness
			message.WriteInt(10); //respects
			message.WriteInt(_petEntity.OwnerId);
			message.WriteInt(10); //days old
			message.WriteString(_petEntity.OwnerUsername); //owner name
			message.WriteInt(0); //rarity
			message.WriteBoolean(false); //sadle
			message.WriteBoolean(false); //riding horse
			message.WriteInt(0); //?
			message.WriteInt(0); //can ride
			message.WriteBoolean(false); //can breed
			message.WriteBoolean(false); //fully grown
			message.WriteBoolean(false); //can revive
			message.WriteInt(0); //rarity
			message.WriteInt(0); //??
			message.WriteInt(0); //??
			message.WriteInt(0); //??
			message.WriteBoolean(false); //public breed?
			return message;
        }
    }
}
