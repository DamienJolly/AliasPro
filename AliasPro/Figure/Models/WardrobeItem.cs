using AliasPro.API.Database;
using AliasPro.API.Figure.Models;
using AliasPro.Network.Protocol;
using AliasPro.Players.Types;
using System.Data.Common;

namespace AliasPro.Figure.Models
{
    internal class WardrobeItem : IWardrobeItem
	{
        public WardrobeItem(DbDataReader reader)
        {
			SlotId = reader.ReadData<int>("slot_id");
			Figure = reader.ReadData<string>("figure");
			Gender = reader.ReadData<string>("gender") == "m" ? PlayerGender.MALE : PlayerGender.FEMALE;
		}

		public WardrobeItem(int slotId, string figure, PlayerGender gender)
        {
			SlotId = slotId;
			Figure = figure;
			Gender = gender;
        }

        public void Compose(ServerPacket message)
        {

        }

        public int SlotId { get; set; }
        public string Figure { get; set; }
        public PlayerGender Gender { get; set; }
    }
}