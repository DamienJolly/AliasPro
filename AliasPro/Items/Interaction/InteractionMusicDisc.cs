using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Interaction
{
    public class InteractionMusicDisc : ItemInteraction
    {
        public int SongId = 0;

        public InteractionMusicDisc(IItem item)
            : base(item)
        {
            int.TryParse(Item.ItemData.ExtraData, out SongId);
        }

        public override void ComposeExtraData(ServerMessage message)
        {
			message.WriteInt(0);
            message.WriteString(SongId + "");
        }
    }
}
