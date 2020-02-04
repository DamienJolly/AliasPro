using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Packets.Events
{
    public class HandleDoorbellEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.HandleDoorbellMessageEvent;

		private readonly IPlayerController _playerController;

		public HandleDoorbellEvent(IPlayerController playerController)
		{
			_playerController = playerController;
		}

		public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
            IRoom room = session.CurrentRoom;
            if (room == null) return;

			string username = clientPacket.ReadString();
			bool accepted = clientPacket.ReadBoolean();

			if (!_playerController.TryGetPlayer(username, out IPlayer targetPlayer)) 
				return;

			if (accepted)
			{
				foreach (BaseEntity entity in room.Entities.Entities)
				{
					if (!(entity is PlayerEntity playerEntity)) continue;

					if (!room.Rights.HasRights(playerEntity.Player.Id)) continue;

					await playerEntity.Session.SendPacketAsync(new RoomAccessibleComposer(session.Player.Username));
				}

				await targetPlayer.Session.SendPacketAsync(new RoomAccessibleComposer(string.Empty));
			}
			else
			{
				foreach (BaseEntity entity in room.Entities.Entities)
				{
					if (!(entity is PlayerEntity playerEntity)) continue;

					if (!room.Rights.HasRights(playerEntity.Player.Id)) continue;

					await playerEntity.Session.SendPacketAsync(new RoomAccessDeniedComposer(session.Player.Username));
				}

				await targetPlayer.Session.SendPacketAsync(new RoomAccessDeniedComposer(string.Empty));
			}
		}
    }
}
