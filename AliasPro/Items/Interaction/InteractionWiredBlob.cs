using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Games.Types;

namespace AliasPro.Items.Interaction
{
	public class InteractionWiredBlob : ItemInteraction
	{

		public InteractionWiredBlob(IItem item)
			: base(item)
		{
			Item.ExtraData = "1";
		}

		public override void ComposeExtraData(ServerMessage message)
		{
			message.WriteInt(0);
			message.WriteString(Item.ExtraData);
		}

		public override void OnPlaceItem()
		{
			Item.ExtraData = "1";
		}

		public override async void OnUserWalkOn(BaseEntity entity)
		{
			if (Item.ExtraData == "1")
				return;

			if (entity.GamePlayer == null)
				return;

			if (entity.GamePlayer.Game.State != GameState.RUNNING)
				return;

			entity.GamePlayer.Points += 1;
			Room.Items.TriggerWired(WiredInteractionType.SCORE_ACHIEVED, entity.GamePlayer.Team.TotalPoints);
			Item.ExtraData = "1";
			await Room.SendPacketAsync(new FloorItemUpdateComposer(Item));
		}

		public override async void OnUserInteract(BaseEntity entity, int state)
		{
			if (Item.ExtraData == "1")
				return;

			if (entity is PlayerEntity playerEntity)
				if (!Room.Rights.HasRights(playerEntity.Player.Id)) 
					return;

			Item.ExtraData = "1";
			await Room.SendPacketAsync(new FloorItemUpdateComposer(Item));
		}
	}
}
