using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;

namespace AliasPro.Items.Interaction
{
    public class InteractionBackgroundToner : ItemInteraction
	{
		public bool Active = false;
		public int Hue = 126;
		public int Saturation = 126;
		public int Brightness = 126;

        public InteractionBackgroundToner(IItem item)
			: base(item)
		{
			string[] data = Item.ExtraData.Split(":");
			if (data.Length == 4)
			{
				Active = bool.Parse(data[0]);
				Hue = int.Parse(data[0]);
				Saturation = int.Parse(data[1]);
				Brightness = int.Parse(data[2]);
			}
		}

		public override void ComposeExtraData(ServerMessage message)
		{
			message.WriteInt(5);
			message.WriteInt(4);
            message.WriteInt(Active ? 1 : 0);
            message.WriteInt(Hue);
            message.WriteInt(Saturation);
            message.WriteInt(Brightness);
        }
        
        public async override void OnUserInteract(BaseEntity entity, int state)
        {
			Active = !Active;
			Item.ExtraData = Active ? "1" : "0" + ":" + Hue + ":" + Saturation + ":" + Brightness;
			await Item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(Item));
		}
    }
}
