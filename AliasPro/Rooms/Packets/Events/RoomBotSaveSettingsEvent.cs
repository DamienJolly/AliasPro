using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Figure.Packets.Composers;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Utilities;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomBotSaveSettingsEvent : IMessageEvent
    {
        public short Header => Incoming.RoomBotSaveSettingsMessageEvent;

		private readonly IRoomController _roomController;

		public RoomBotSaveSettingsEvent(IRoomController roomController)
		{
			_roomController = roomController;
		}

		public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;
            if (room == null) 
				return;

			if (room.OwnerId != session.Player.Id)
				return;

			int botId = message.ReadInt();
			if (!room.Entities.TryGetEntityById(botId, out BaseEntity entity))
				return;

			if (!(entity is BotEntity botEntity))
				return;

			int settingId = message.ReadInt();

			switch (settingId)
			{
				case 1:
					botEntity.Figure = session.Player.Figure;
					botEntity.Gender = session.Player.Gender;
					await room.SendPacketAsync(new UpdateEntityDataComposer(botEntity));
					break;

				case 2:
					//to-do
					break;

				case 3:
						botEntity.CanWalk = !botEntity.CanWalk;
					break;

				case 4:
					if (botEntity.DanceId != 0)
						botEntity.DanceId = 0;
					else
						botEntity.DanceId = Randomness.RandomNumber(1, 4);

					await room.SendPacketAsync(new UserDanceComposer(botEntity));
					break;

				case 5:
					string name = message.ReadString();
					if (name.Length > 25)
						return;

					botEntity.Name = name;
					await room.SendPacketAsync(new EntitiesComposer(botEntity));
					break;
			}
		}
    }
}
