using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Protocol;
using AliasPro.Rooms.Entities;
using System;

namespace AliasPro.Items.Interaction
{
    public class InteractionGameTimer : IItemInteractor
    {
        private readonly IItem _item;

        private bool _active = false;
        private int _tick = 0;
        private int _timer = 30;

        public InteractionGameTimer(IItem item)
        {
            _item = item;

            if (int.TryParse(_item.ExtraData, out int timer))
                _timer = timer;

            _tick = _timer * 2;
        }

        public void Compose(ServerPacket message)
        {
            double timeLeft = Math.Ceiling(_tick / 2.0);

			message.WriteInt(1);
			message.WriteInt(0);
            message.WriteString(timeLeft.ToString());
        }

		public void OnPlaceItem()
		{

		}

		public void OnPickupItem()
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

			switch (state)
            {
                case 1:
                    {
                        if (_active)
                        {
                            _active = false;
                            _item.CurrentRoom.Game.EndGame();
                        }
                        else
                        {
                            if (_item.CurrentRoom.Game.GameStarted) return;

                            _active = true;
                            _item.CurrentRoom.Game.StartGame();
                        }

                        _tick = _timer * 2;

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

            await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
        }

        public async void OnCycle()
        {
            if (_active)
            {
                if (_tick <= 0 ||
                    !_item.CurrentRoom.Game.GameStarted)
                {
                    _active = false;
                    _tick = _timer * 2;
                    _item.CurrentRoom.Game.EndGame();
                }
                _tick--;

                await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
            }
        }
    }
}
