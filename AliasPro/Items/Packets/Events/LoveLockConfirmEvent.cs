using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Entities;

namespace AliasPro.Items.Packets.Events
{
    public class LoveLockConfirmEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.LoveLockConfirmMessageEvent;
        
        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
			uint itemId = (uint)clientPacket.ReadInt();
			bool confirmed = clientPacket.ReadBool();

			if (!confirmed)
				return;

			IRoom room = session.CurrentRoom;

			if (room == null) return;

			if (session.Entity == null) return;

			if (room.Items.TryGetItem(itemId, out IItem item))
			{
				if (item.ItemData.Type != "s") return;

				if (item.ItemData.InteractionType != ItemInteractionType.LOVE_LOCK) return;

				if (item.InteractingPlayer.Id != session.Entity.Id && item.InteractingPlayerTwo.Id != session.Entity.Id)
					return;

				BaseEntity targetEntity;
				if (item.InteractingPlayer.Id == session.Entity.Id)
					targetEntity = item.InteractingPlayerTwo;
				else if (item.InteractingPlayerTwo.Id == session.Entity.Id)
					targetEntity = item.InteractingPlayer;
				else
					return;

				if (targetEntity == null)
					return;

				if (targetEntity is PlayerEntity playerEntity)
				{
					await playerEntity.Player.Session.SendPacketAsync(new LoveLockFriendConfirmedComposer(item));
					await playerEntity.Player.Session.SendPacketAsync(new LoveLockFinishedComposer(item));
				}

				await session.SendPacketAsync(new LoveLockFinishedComposer(item));

				item.ExtraData = 
					session.Entity.Name + ";" + 
					targetEntity.Name + ";" + 
					session.Entity.Figure + ";" + 
					targetEntity.Figure + ";" + 
					"30-1-2019";

				await room.SendAsync(new FloorItemUpdateComposer(item));
			}
		}
    }
}
