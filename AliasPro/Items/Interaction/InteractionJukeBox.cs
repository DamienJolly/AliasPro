using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;

namespace AliasPro.Items.Interaction
{
    public class InteractionJukeBox : ItemInteraction
    {
        public InteractionJukeBox(IItem item)
            : base(item)
        {

        }

        public override void ComposeExtraData(ServerMessage message)
        {
            message.WriteInt(0);
            message.WriteString(Item.ExtraData);
        }

		public override void OnPickupItem()
		{
            Item.CurrentRoom.Trax.Stop();
            Item.ExtraData = "0";
        }

        public async override void OnUserInteract(BaseEntity entity, int state)
        {
            IRoom room = Item.CurrentRoom;
            if (room == null)
                return;

            if (state == -1)
                return;

            if (!room.Trax.CurrentlyPlaying)
            {
                room.Trax.Play();

                if (room.Trax.CurrentlyPlaying)
                    Item.ExtraData = "1";
                else
                    Item.ExtraData = "0";
            }
            else
            {
                room.Trax.Stop();
                Item.ExtraData = "0";
            }

            await room.SendPacketAsync(new FloorItemUpdateComposer(Item));
        }

        public override void OnCycle()
        {
            if (Item.ExtraData == "1" && !Item.CurrentRoom.Trax.CurrentlyPlaying)
                OnUserInteract(null, 0);
        }
    }
}
