using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Models;
using AliasPro.Items.Types;
using AliasPro.Rooms.Models;
using AliasPro.Utilities;
using System.Collections.Generic;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionTriggerStacks : WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.CALL_STACKS;

        private bool _active = false;
        private object[] _args;
        private int _tick = 0;

        public WiredInteractionTriggerStacks(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool Execute(params object[] args)
        {
            if (!_active)
            {
                _active = true;
                _args = args;
                _tick = WiredData.Delay;
            }
            return true;
        }

        public override void OnCycle()
        {
            if (_active)
            {
                if (_tick <= 0)
                {
                    IList<IRoomTile> stacks = new List<IRoomTile>();

                    foreach (WiredItemData itemData in WiredData.Items.Values)
                    {
                        if (!Room.Items.TryGetItem(itemData.ItemId, out IItem item)) continue;

                        IList<IRoomTile> roomTiles = Room.RoomGrid.GetTilesFromItem(item.Position.X, item.Position.Y, item);

                        foreach(IRoomTile roomTile in roomTiles)
                        {
                            if (!stacks.Contains(roomTile))
                                stacks.Add(roomTile);
                        }
                    }

                    foreach (IRoomTile roomTile in stacks)
                    {
                        Room.Items.TriggerEffects(roomTile, _args);
                    }

                    _active = false;
                }
                _tick--;
            }
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
