using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Rooms.Entities;
using System;

namespace AliasPro.Items.Interaction
{
    public class InteractionGameTimer : ItemInteraction
    {
        private bool _active = false;
        private bool _isPaused = false;
        private int _tick = 0;
        private int _timer = 30;

        public InteractionGameTimer(IItem item)
            : base(item)
        {
            if (int.TryParse(Item.ExtraData, out int timer))
                _timer = timer;

            _tick = _timer * 2;
        }

        public override void ComposeExtraData(ServerMessage message)
        {
			double timeLeft = Math.Ceiling(_tick / 2.0);

			message.WriteInt(0);
            message.WriteDouble(timeLeft);
        }

        public async override void OnUserInteract(BaseEntity entity, int state)
        {
			if (entity is PlayerEntity playerEntity)
				if (!Item.CurrentRoom.Rights.HasRights(playerEntity.Player.Id)) return;

            //todo: activate with wired

            switch (state)
            {
                case 1:
                    {
                        if (_active)
                        {
                            _isPaused = !_isPaused;

                            if (_isPaused)
                                Item.CurrentRoom.Game.PauseGames();
                            else
                                Item.CurrentRoom.Game.UnpauseGames();
                        }
                        else
                        {
                            _active = true;
                            _isPaused = false;
                            _tick = _timer * 2;

                            Item.CurrentRoom.Game.StartGames();
                            Item.CurrentRoom.Items.TriggerWired(WiredInteractionType.GAME_STARTS);

                            foreach (IItem wiredBlob in Room.Items.GetItemsByType(ItemInteractionType.WIRED_BLOB))
                            {
                                wiredBlob.ExtraData = "0";
                                await Room.SendPacketAsync(new FloorItemUpdateComposer(wiredBlob));
                            }
                        }

                        break;
                    }
                case 2:
                    {
                        if (_active) return;

                        _timer += 30;

                        if (_timer > 600)
                            _timer = 30;

                        _tick = _timer * 2;
                        Item.ExtraData = _timer.ToString();

                        break;
                    }
            }

            await Item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(Item));
        }

        public async override void OnCycle()
        {
            if (_active && !_isPaused)
            {
                if (_tick <= 0)
                {
                    _active = false;
                    _tick = _timer * 2;

                    Item.CurrentRoom.Game.EndGames();
                    Item.CurrentRoom.Items.TriggerWired(WiredInteractionType.GAME_ENDS);

                    foreach (IItem wiredBlob in Room.Items.GetItemsByType(ItemInteractionType.WIRED_BLOB))
                    {
                        wiredBlob.ExtraData = "1";
                        await Room.SendPacketAsync(new FloorItemUpdateComposer(wiredBlob));
                    }
                }
                _tick--;

                await Item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(Item));
            }
        }
    }
}
