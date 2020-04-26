using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Rooms.Entities;
using System;

namespace AliasPro.Items.Interaction
{
    public class InteractionGameTimer : IItemInteractor
    {
        private readonly IItem _item;

        private bool _active = false;
        private bool _isPaused = false;
        private int _tick = 0;
        private int _timer = 30;

        public InteractionGameTimer(IItem item)
        {
            _item = item;

            if (int.TryParse(_item.ExtraData, out int timer))
                _timer = timer;

            _tick = _timer * 2;
        }

		public void Compose(ServerMessage message, bool tradeItem)
		{
			double timeLeft = Math.Ceiling(_tick / 2.0);

			if (!tradeItem)
				message.WriteInt(1);
			message.WriteInt(0);
            message.WriteDouble(timeLeft);
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
				if (!_item.CurrentRoom.Rights.HasRights(playerEntity.Player.Id)) return;

            //todo: activate with wired

            switch (state)
            {
                case 1:
                    {
                        if (_active)
                        {
                            _isPaused = !_isPaused;

                            if (_isPaused)
                                _item.CurrentRoom.Game.PauseGames();
                            else
                                _item.CurrentRoom.Game.UnpauseGames();
                        }
                        else
                        {
                            _active = true;
                            _isPaused = false;
                            _tick = _timer * 2;

                            _item.CurrentRoom.Game.StartGames();
                            _item.CurrentRoom.Items.TriggerWired(WiredInteractionType.GAME_STARTS);
                            _item.CurrentRoom.Items.TriggerWired(WiredInteractionType.AT_GIVEN_TIME);
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
                        _item.ExtraData = _timer.ToString();

                        break;
                    }
            }

            await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
        }

        public async void OnCycle()
        {
            if (_active && !_isPaused)
            {
                if (_tick <= 0)
                {
                    _active = false;
                    _tick = _timer * 2;

                    _item.CurrentRoom.Game.EndGames();
                    _item.CurrentRoom.Items.TriggerWired(WiredInteractionType.GAME_ENDS);
                }
                _tick--;

                await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
            }
        }
    }
}
