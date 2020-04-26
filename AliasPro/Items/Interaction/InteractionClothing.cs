using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Interaction
{
    public class InteractionClothing : ItemInteraction
    {
        public InteractionClothing(IItem item)
            : base(item)
        {

        }

        public override void ComposeExtraData(ServerMessage message)
        {
            message.WriteInt(2);
            message.WriteInt(1);
            message.WriteString("");
        }
    }
}
