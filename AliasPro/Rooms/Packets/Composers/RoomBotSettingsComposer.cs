using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomBotSettingsComposer : IMessageComposer
    {
		private readonly BaseEntity _entity;
		private readonly int _settingId;

        public RoomBotSettingsComposer(BaseEntity entity, int settingId)
        {
			_entity = entity;
			_settingId = settingId;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomBotSettingsMessageComposer);
			message.WriteInt(_entity.Id);
			message.WriteInt(_settingId);
			switch (_settingId)
			{
				case 2:
					message.WriteString(string.Empty); //speech
					break;
				case 5:
					message.WriteString(_entity.Name);
					break;
			}
			return message;
        }
    }
}
