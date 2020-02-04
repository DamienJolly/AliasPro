using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Entities;

namespace AliasPro.Rooms.Packets.Composers
{
    public class PetInformationComposer : IMessageComposer
    {
        private readonly PetEntity _petEntity;

        public PetInformationComposer(PetEntity petEntity)
        {
			_petEntity = petEntity;
        }

        public ServerMessage Compose()
        {
			ServerMessage message = new ServerMessage(Outgoing.PetInformationMessageComposer);
			message.WriteInt(_petEntity.Id);
			message.WriteString(_petEntity.Name);
			message.WriteInt(_petEntity.PetLevel);
			message.WriteInt(20); //max level, config?
			message.WriteInt(_petEntity.Experience);
			message.WriteInt(_petEntity.ExperienceLeft);
			message.WriteInt(_petEntity.Energy);
			message.WriteInt(100); //max energy, config?
			message.WriteInt(_petEntity.Happyness);
			message.WriteInt(100); //max happyness, config?
			message.WriteInt(_petEntity.Respect);
			message.WriteInt((int)_petEntity.OwnerId);
			message.WriteInt(_petEntity.DaysOld);
			message.WriteString(_petEntity.OwnerUsername);
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
