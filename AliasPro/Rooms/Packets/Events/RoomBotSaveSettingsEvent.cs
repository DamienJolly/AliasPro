using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Figure.Packets.Composers;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Utilities;

namespace AliasPro.Rooms.Packets.Events
{
    public class RoomBotSaveSettingsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RoomBotSaveSettingsMessageEvent;

		private readonly IRoomController _roomController;

		public RoomBotSaveSettingsEvent(IRoomController roomController)
		{
			_roomController = roomController;
		}

		public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null) return;

			if (room.OwnerId != session.Player.Id)
				return;

			int botId = clientPacket.ReadInt();
			if (!room.Entities.TryGetEntityById(botId, out BaseEntity entity))
				return;

			if (!(entity is BotEntity botEntity))
				return;

			int settingId = clientPacket.ReadInt();

			switch (settingId)
			{
				case 1:
					botEntity.Figure = session.Player.Figure;
					botEntity.Gender = session.Player.Gender;
					await room.SendAsync(new UpdateEntityDataComposer(botEntity));
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

					await room.SendAsync(new UserDanceComposer(botEntity));
					break;

				case 5:
					string name = clientPacket.ReadString();
					if (name.Length > 25)
						return;

					botEntity.Name = name;
					await room.SendAsync(new EntitiesComposer(botEntity));
					break;
			}
		}
    }
}
