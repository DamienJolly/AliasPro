using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Rooms.Entities;

namespace AliasPro.Items.Interaction
{
    public class InteractionDefault : ItemInteraction
    {
        private readonly IItem _item;

        public InteractionDefault(IItem item)
            : base(item)
        {
            
        }

		public override void ComposeExtraData(ServerMessage message)
		{
			message.WriteInt(0);
            message.WriteString(_item.ExtraData);
        }
        
        public async override void OnUserInteract(BaseEntity entity, int state)
        {
            int modes = _item.ItemData.Modes - 1;
            if (modes <= 0)
                return;

            if (entity is PlayerEntity playerEntity)
				if (!_item.CurrentRoom.Rights.HasRights(playerEntity.Player.Id)) return;


            if (_item.ItemData.Modes > 0)
            {
                if (!int.TryParse(_item.ExtraData, out int currentState))
                {
                    currentState = 0;
                }

                _item.ExtraData = "" + (currentState + 1) % _item.ItemData.Modes;
            }

			await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
        }
    }
}
