using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Items.Models;
using AliasPro.Items.Types;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Rooms.Types;

namespace AliasPro.Items.WiredInteraction
{
    public class WiredInteractionMessage : IWiredInteractor
    {
        private readonly IItem _item;
        private readonly WiredEffectType _type = WiredEffectType.SHOW_MESSAGE;

        private bool _active = false;
        private BaseEntity _target = null;
        private int _tick = 0;

        public IWiredData WiredData { get; set; }

        public WiredInteractionMessage(IItem item)
        {
            _item = item;
            WiredData =
                new WiredData((int)_type, _item.ExtraData);
        }
        
        public bool OnTrigger(params object[] args)
        {
            if (!_active)
            {
                _active = true;
                _tick = WiredData.Delay;

                if (args.Length != 0)
                    _target = (BaseEntity)args[0];
            }
            return true;
        }

        public async void OnCycle()
        {
            if (_active)
            {
                if (_tick <= 0)
                {
                    if (_target != null)
                    {
                        if (_target is PlayerEntity userEntity)
                        {
                            await userEntity.Session.SendPacketAsync(new AvatarChatComposer(
                                userEntity.Id, WiredData.Message, 0, 34, RoomChatType.TALK));
                        }
                    }
                    else
                    {
                        foreach (PlayerEntity entity in _item.CurrentRoom.Entities.Entities)
                        {
                            await entity.Session.SendPacketAsync(new AvatarChatComposer(
                            entity.Id, WiredData.Message, 0, 34, RoomChatType.TALK));
                        }
                    }
                    _active = false;
                }
                _tick--;
            }
        }
    }
}
