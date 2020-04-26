using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;

namespace AliasPro.Items.Interaction
{
    public class InteractionScoreboard : ItemInteraction
    {
        public InteractionScoreboard(IItem item)
            : base(item)
        {

        }

        public override void ComposeExtraData(ServerMessage message)
        {
            message.WriteInt(0);
            message.WriteString(Item.ExtraData);
        }

        public override void OnPlaceItem()
		{
            Item.ExtraData = "";
		}
        
        public async override void OnUserInteract(BaseEntity entity, int state)
        {
            int.TryParse(Item.ExtraData, out int currentScore);

            switch (state)
            {
                default:
                case 3:
                    currentScore = 0; break;
                case 2:
                    currentScore -= 1; break;
                case 1:
                    currentScore += 1; break;
            }

            if (currentScore >= 100)
                currentScore = 0;
            else if (currentScore < 0)
                currentScore = 99;

            Item.ExtraData = $"{currentScore}";
			await Item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(Item));
        }
    }
}
