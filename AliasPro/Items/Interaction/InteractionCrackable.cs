using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Tasks;
using AliasPro.Rooms.Entities;
using AliasPro.Tasks;

namespace AliasPro.Items.Interaction
{
    public class InteractionCrackable : ItemInteraction
    {
        public bool Cracked = false;

        public InteractionCrackable(IItem item) 
            : base(item)
        {

        }

        public override void ComposeExtraData(ServerMessage message)
        {
			message.WriteInt(7);

            int.TryParse(Item.ExtraData, out int hits);
            int totalHits = 0;
            int crackState = 0;

            if (Program.GetService<IItemController>().TryGetCrackableDataById((int)Item.ItemData.Id, out ICrackableData crackable))
            {
                totalHits = crackable.Count;
                crackState = crackable.CalculateCrackState(hits, Item.ItemData.Modes - 1);
            }

            message.WriteString(crackState + "");
            message.WriteInt(hits);
            message.WriteInt(totalHits);
        }
        
        public async override void OnUserInteract(BaseEntity entity, int state)
        {
            if (!(entity is PlayerEntity playerEntity))
                return;

            if (Cracked)
                return;

            if (Item.PlayerId != playerEntity.Player.Id)
                return;

            if (!Program.GetService<IItemController>().TryGetCrackableDataById((int)Item.ItemData.Id, out ICrackableData crackable))
                return;

            if (crackable.EffectId != 0 && crackable.EffectId != entity.EffectId)
                return;

            if (!Item.CurrentRoom.RoomGrid.TryGetRoomTile(Item.Position.X, Item.Position.Y, out IRoomTile tile))
                return;

            if (!tile.TilesAdjecent(playerEntity.Position))
            {
                IRoomPosition position = tile.PositionInFront(Item.Rotation);
                playerEntity.GoalPosition = position;
                return;
            }

            int.TryParse(Item.ExtraData, out int hits);
            hits++;
            Item.ExtraData = "" + hits;
            await Item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(Item));

            //todo: progress tick achievement

            if (!Cracked && hits == crackable.Count)
            {
                Cracked = true;
                await TaskManager.ExecuteTask(new CrackableExplode(Item, playerEntity.Session, true), 1500);

                //todo: progress cracked achievement
                //todo: subscriptions (for sub boxes)
            }
        }
    }
}
