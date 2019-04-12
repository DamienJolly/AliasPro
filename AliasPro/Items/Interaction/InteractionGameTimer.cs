using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Protocol;
using AliasPro.Room.Models.Entities;
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

            message.WriteInt(0);
            message.WriteString(timeLeft.ToString());
        }

        public void OnUserWalkOn(BaseEntity entity)
        {

        }

        public void OnUserWalkOff(BaseEntity entity)
        {

        }

        public async void OnUserInteract(BaseEntity entity, int state)
        {
            switch (state)
            {
                case 1:
                    {
                        if (_active)
                        {
                            _active = false;
                            _item.CurrentRoom.GameHandler.EndGame();
                        }
                        else
                        {
                            if (_item.CurrentRoom.GameHandler.GameStarted) return;

                            _active = true;
                            _item.CurrentRoom.GameHandler.StartGame();
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
                    !_item.CurrentRoom.GameHandler.GameStarted)
                {
                    _active = false;
                    _tick = _timer * 2;
                    _item.CurrentRoom.GameHandler.EndGame();
                }
                _tick--;

                await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
            }
        }
    }
}
