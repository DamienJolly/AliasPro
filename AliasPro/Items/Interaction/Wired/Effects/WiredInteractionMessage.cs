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

        public WiredInteractionMessage(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            BaseEntity target = null;
            if (args.Length != 0)
                target = (BaseEntity)args[0];

            if (target != null)
            {
                if (target is PlayerEntity userEntity)
                {
                    userEntity.Session.SendPacketAsync(new AvatarChatComposer(
                        userEntity.Id, WiredData.Message, 0, 34, RoomChatType.TALK));
                }
            }
            else
            {
                foreach (PlayerEntity entity in Room.Entities.Entities)
                {
                    entity.Session.SendPacketAsync(new AvatarChatComposer(
                        entity.Id, WiredData.Message, 0, 34, RoomChatType.TALK));
                }
            }

            return true;
        }
    }
}
