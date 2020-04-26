using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Items.Interaction
{
    public class InteractionTent : ItemInteraction
	{
		public IList<BaseEntity> TentEntities;

        public InteractionTent(IItem item)
			: base(item)
		{
			TentEntities = new List<BaseEntity>();
		}

		public override void ComposeExtraData(ServerMessage message)
		{
			message.WriteInt(0);
			message.WriteString(Item.ExtraData);
		}

		public override void OnUserWalkOn(BaseEntity entity)
        {
			TentEntities.Add(entity);
		}

        public override void OnUserWalkOff(BaseEntity entity)
        {
			TentEntities.Remove(entity);
		}
    }
}
