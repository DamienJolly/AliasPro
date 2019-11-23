using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Entities;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomBotSettingsComposer : IPacketComposer
    {
		private readonly BaseEntity _entity;
		private readonly int _settingId;

        public RoomBotSettingsComposer(BaseEntity entity, int settingId)
        {
			_entity = entity;
			_settingId = settingId;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomBotSettingsMessageComposer);
			message.WriteInt(_entity.Id);
			message.WriteInt(_settingId);
			switch (_settingId)
			{
				case 2:
					//to-do
					break;
				case 5:
					message.WriteString(_entity.Name);
					break;
			}
			return message;
        }
    }
}
