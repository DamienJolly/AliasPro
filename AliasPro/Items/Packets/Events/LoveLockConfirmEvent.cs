using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Rooms.Entities;
using System;
using System.Threading.Tasks;

namespace AliasPro.Items.Packets.Events
{
    public class LoveLockConfirmEvent : IMessageEvent
    {
        public short Header => Incoming.LoveLockConfirmMessageEvent;
        
        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
			uint itemId = (uint)message.ReadInt();
			bool confirmed = message.ReadBoolean();

			if (!confirmed)
				return;

			IRoom room = session.CurrentRoom;

			if (room == null) 
				return;

			if (session.Entity == null) 
				return;

			if (room.Items.TryGetItem(itemId, out IItem item))
			{
				if (item.ItemData.Type != "s") 
					return;

				if (item.ItemData.InteractionType != ItemInteractionType.LOVE_LOCK) 
					return;

				if(item.InteractingPlayer == null || item.InteractingPlayerTwo == null)
					return;

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
					DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;

				await room.SendPacketAsync(new FloorItemUpdateComposer(item));
			}
		}
    }
}
