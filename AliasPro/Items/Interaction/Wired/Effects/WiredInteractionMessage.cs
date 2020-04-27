using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Items.Types;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Rooms.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionMessage : WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.SHOW_MESSAGE;

        private bool _active = false;
        private BaseEntity _target = null;
        private int _tick = 0;

        public WiredInteractionMessage(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool Execute(params object[] args)
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

        public async override void OnCycle()
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
                        foreach (PlayerEntity entity in Room.Entities.Entities)
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
