using AliasPro.API.Items.Models;
using AliasPro.API.Messenger;
using AliasPro.Items.Types;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Models;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Rooms.Types;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionKickPlayer : WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.KICK_USER;

        public WiredInteractionKickPlayer(IItem item)
            : base(item, (int)_type)
        {

        }

        public override bool TryHandle(params object[] args)
        {
            PlayerEntity target = null;
            if (args.Length != 0)
                target = (PlayerEntity)args[0];

            if (target == null)
                return false;

            if (Room.Rights.IsOwner(target.Player.Id))
                return false;

            if (Room.RoomModel.IsCustom /*|| room.IsPublic  */)
            {
                Room.RemoveEntity(target);
            }
            else
            {
                target.Session.CurrentRoom = null;
                target.IsKicked = true;

                target.GoalPosition = new RoomPosition(
                    Room.RoomModel.DoorX,
                    Room.RoomModel.DoorY,
                    Room.RoomModel.DoorZ
                );
            }

            if (target.Session.Player.Messenger != null)
                Program.GetService<IMessengerController>().UpdateStatusAsync(target.Session.Player, target.Session.Player.Messenger.Friends);

            if (!string.IsNullOrWhiteSpace(WiredData.Message))
                target.Session.SendPacketAsync(new AvatarChatComposer(
                        target.Id, WiredData.Message, 0, 34, RoomChatType.WHISPER));

            //target.Session.SendPacketAsync(new GenericErrorComposer(GenericErrorComposer.KICKED_OUT_OF_THE_ROOM));

            return true;
        }
    }
}
