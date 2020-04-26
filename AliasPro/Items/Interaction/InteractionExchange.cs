using AliasPro.API.Items.Models;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Packets.Composers;
using AliasPro.Rooms.Entities;

namespace AliasPro.Items.Interaction
{
    public class InteractionExchange: ItemInteraction
	{
        public InteractionExchange(IItem item)
			: base(item)
		{

        }

		public override void ComposeExtraData(ServerMessage message)
		{
			message.WriteInt(0);
			message.WriteString(Item.ExtraData);
		}

        public async override void OnUserInteract(BaseEntity entity, int state)
        {
			if (entity is PlayerEntity playerEntity)
				if (!Item.CurrentRoom.Rights.IsOwner(playerEntity.Player.Id)) return;

			if (!int.TryParse(Item.ItemData.ExtraData, out int amount))
			{
				return;
			}

			if (entity is PlayerEntity userEntity)
			{ 
				if (userEntity.Player.Currency.TryGetCurrency(-1, out IPlayerCurrency currency))
				{
					currency.Amount += amount;
					await userEntity.Player.Session.SendPacketAsync(new UserCreditsComposer(currency.Amount));
				}
			}
        }
    }
}
