using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Players.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Packets.Composers;
using AliasPro.Rooms.Entities;

namespace AliasPro.Items.Interaction
{
    public class InteractionExchange: IItemInteractor
    {
        private readonly IItem _item;

        public InteractionExchange(IItem item)
        {
            _item = item;
        }

		public void Compose(ServerMessage message, bool tradeItem)
		{
			if (!tradeItem)
				message.WriteInt(1);
			message.WriteInt(0);
            message.WriteString(_item.ExtraData);
        }

		public void OnPlaceItem()
		{

		}

		public void OnPickupItem()
		{

		}

		public void OnMoveItem()
		{

		}

		public void OnUserWalkOn(BaseEntity entity)
        {

        }

        public void OnUserWalkOff(BaseEntity entity)
        {

        }
        
        public async void OnUserInteract(BaseEntity entity, int state)
        {
			if (entity is PlayerEntity playerEntity)
				if (!_item.CurrentRoom.Rights.IsOwner(playerEntity.Player.Id)) return;

			if (!int.TryParse(_item.ItemData.ExtraData, out int amount))
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

        public void OnCycle()
        {

        }
    }
}
