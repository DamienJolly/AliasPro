using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;

namespace AliasPro.Items.Interaction
{
    public class InteractionScoreboard : IItemInteractor
    {
        private readonly IItem _item;

        public InteractionScoreboard(IItem item)
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
            _item.ExtraData = "";
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
            int.TryParse(_item.ExtraData, out int currentScore);

            switch (state)
            {
                default:
                case 3:
                    currentScore = 0; break;
                case 2:
                    currentScore -= 1; break;
                case 1:
                    currentScore += 1; break;
            }

            if (currentScore >= 100)
                currentScore = 0;
            else if (currentScore < 0)
                currentScore = 99;

            _item.ExtraData = $"{currentScore}";
			await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
        }

        public void OnCycle()
        {

        }
    }
}
