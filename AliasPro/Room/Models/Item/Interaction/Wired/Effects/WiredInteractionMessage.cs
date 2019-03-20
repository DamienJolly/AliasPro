using AliasPro.Item.Models;
using AliasPro.Room.Models.Entities;
using AliasPro.Room.Packets.Outgoing;

namespace AliasPro.Room.Models.Item.Interaction.Wired
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
        
        public void OnTrigger(params object[] args)
        {
            if (!_active)
            {
                _active = true;
                _tick = WiredData.Delay;

                if (args.Length != 0)
                    _target = (BaseEntity)args[0];
            }
        }

        public async void OnCycle()
        {
            if (_active)
            {
                if (_tick <= 0)
                {
                    if (_target != null)
                    {
                        if (_target is UserEntity userEntity)
                        {
                            await userEntity.Session.SendPacketAsync(new AvatarChatComposer(
                                userEntity.Id, WiredData.Message, 0, 34));
                        }
                    }
                    else
                    {
                        foreach (UserEntity entity in _item.CurrentRoom.EntityHandler.Entities)
                        {
                            await entity.Session.SendPacketAsync(new AvatarChatComposer(
                            entity.Id, WiredData.Message, 0, 34));
                        }
                    }
                    _active = false;
                }
                _tick--;
            }
        }
    }
}
