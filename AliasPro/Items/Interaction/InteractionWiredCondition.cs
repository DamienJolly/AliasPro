using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Packets.Composers;

namespace AliasPro.Items.Interaction
{
    public class InteractionWiredCondition : InteractionWired
    {

        public InteractionWiredCondition(IItem item)
            : base(item)
        {

        }

        public async override void OnUserInteract(BaseEntity entity, int state)
        {
            if (entity is PlayerEntity playerEntity)
			{
				if (!Item.CurrentRoom.Rights.HasRights(playerEntity.Player.Id)) return;

                await playerEntity.Session.SendPacketAsync(new WiredConditionDataComposer(Item));
            }
        }
    }
}
