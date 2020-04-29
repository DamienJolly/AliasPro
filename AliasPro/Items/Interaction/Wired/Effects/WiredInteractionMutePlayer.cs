using AliasPro.API.Items.Models;
using AliasPro.Items.Types;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Rooms.Types;
using AliasPro.Utilities;

namespace AliasPro.Items.Interaction.Wired
{
    public class WiredInteractionMutePlayer : WiredInteraction
    {
        private static readonly WiredEffectType _type = WiredEffectType.MUTE_TRIGGER;

        public WiredInteractionMutePlayer(IItem item)
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

            if (Length < 0 || Length > 10)
                return false;

            if (Room.Rights.HasRights(target.Player.Id))
                return false;

            Room.Mute.MutePlayer((int)target.Player.Id, (int)UnixTimestamp.Now + (Length * 60));

            if (Length != 0)
                target.Session.SendPacketAsync(new MutedWhisperComposer(Length * 60));

            if (!string.IsNullOrWhiteSpace(WiredData.Message))
                target.Session.SendPacketAsync(new AvatarChatComposer(
                        target.Id, WiredData.Message, 0, 34, RoomChatType.WHISPER));

            return true;
        }

        private int Length =>
            (WiredData.Params.Count <= 0) ? 0 : WiredData.Params[0];
    }
}
