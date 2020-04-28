using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Rooms.Models;
using AliasPro.Utilities;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionMoveRotate: WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.MOVE_ROTATE;

        public WiredInteractionMoveRotate(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            foreach (WiredItemData itemData in WiredData.Items.Values)
            {
                if (!Room.Items.TryGetItem(itemData.ItemId, out IItem item)) 
                    continue;

                int movementMode = 0;
                int rotationMode = 0;

                if (WiredData.Params.Count >= 2)
                {
                    movementMode = WiredData.Params[0];
                    rotationMode = WiredData.Params[1];
                }

                IRoomPosition newPos = HandleMovement(movementMode, item.Position);
                int newRot = HandleRotation(rotationMode, item.Rotation);

                Room.Items.TriggerWired(WiredInteractionType.COLLISION, newPos);

                if (!Room.RoomGrid.TryGetRoomTile(newPos.X, newPos.Y, out IRoomTile roomTile) ||
                    !Room.RoomGrid.CanRollAt(newPos.X, newPos.Y, item))
                    continue;

                newPos.Z = roomTile.Height;
                Room.SendPacketAsync(new FloorItemOnRollerComposer(item, newPos, 0));

                Room.RoomGrid.RemoveItem(item);
                item.Position = newPos;
                item.Rotation = newRot;
                Room.RoomGrid.AddItem(item);
            }
            return true;
        }

        private int HandleRotation(int mode, int rotation)
        {
            int randomNum = Randomness.RandomNumber(1, 4);

            switch (mode)
            {
                case 1: rotation += 2; break;
                case 2: rotation -= 2; break;
                case 3:
                    {
                        if (randomNum % 2 == 0)
                            rotation += 2;
                        else
                            rotation -= 2;
                        break;
                    }
            }

            if (rotation > 6) rotation = 0;
            if (rotation < 0) rotation = 6;

            return rotation;
        }

        private IRoomPosition HandleMovement(int Mode, IRoomPosition position)
        {
            IRoomPosition newPos = 
                new RoomPosition(position.X, position.Y, position.Z);
            int randomNum = Randomness.RandomNumber(1, 4);

            switch (Mode)
            {
                case 1:
                    {
                        switch (randomNum)
                        {
                            case 1: newPos.X += 1; break;
                            case 2: newPos.X -= 1; break;
                            case 3: newPos.Y += 1; break;
                            case 4: newPos.Y -= 1; break;
                        }
                        break;
                    }
                case 2:
                    {
                        if (randomNum % 2 == 0)
                            newPos.X += 1;
                        else
                            newPos.X -= 1;
                        break;
                    }
                case 3:
                    {
                        if (randomNum % 2 == 0)
                            newPos.Y += 1;
                        else
                            newPos.Y -= 1;
                        break;
                    }
                case 4: newPos.Y -= 1; break;
                case 5: newPos.X += 1; break;
                case 6: newPos.Y += 1; break;
                case 7: newPos.X -= 1; break;
            }

            return newPos;
        }
    }
}
