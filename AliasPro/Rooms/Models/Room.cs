using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Items.Types;
using AliasPro.Rooms.Components;
using AliasPro.Rooms.Entities;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Rooms.Cycles;
using System;
using System.Threading.Tasks;
using AliasPro.Landing.Packets.Composers;
using AliasPro.API.Groups.Models;
using AliasPro.Groups.Packets.Composers;

namespace AliasPro.Rooms.Models
{
    internal class Room : RoomData, IRoom, IDisposable
    {
        public EntitiesComponent Entities { get; set; }
        public ItemsComponent Items { get; set; }
        public RightsComponent Rights { get; set; }
        public GameComponent Game { get; set; }
        public RoomGrid RoomGrid { get; set; }
        public RoomCycle RoomCycle { get; set; }
        public int IdleTimer { get; set; } = 0;

        internal Room(IRoomData roomData)
            : base(roomData)
        {

        }

        public async void Cycle()
        {
            try
            {
                foreach (BaseEntity entity in Entities.Entities)
                    entity.RoomEntityCycle.Cycle();

                foreach (IItem item in Items.Items)
                    item.Interaction.OnCycle();

                if (Entities.Entities.Count <= 0)
                    IdleTimer++;
                else
                    await SendAsync(new EntityUpdateComposer(Entities.Entities));
            }
            catch { }
        }

		public async void OnChat(string text, int colour, BaseEntity entity)
        {
            if (colour == 1 || colour == -1 || colour == 2)
            {
                colour = 0;
            }

            if (text.Length > 100)
            {
                text = text.Substring(0, 100);
            }

            Items.TriggerWired(WiredInteractionType.SAY_SOMETHING, entity, text);

            foreach (BaseEntity targetEntity in Entities.Entities)
            {
                if (targetEntity == entity) continue;

                int newDir = targetEntity.Position.CalculateDirection(entity.Position.X, entity.Position.Y);

                if (Math.Abs(newDir - targetEntity.BodyRotation) <= 2)
                    targetEntity.SetRotation(newDir, true);

                targetEntity.DirOffsetTimer = 0;
            }

            await SendAsync(new AvatarChatComposer(entity.Id, text, 0, colour));
        }

		public async Task AddEntity(BaseEntity entity)
        {
            Entities.AddEntity(entity);
            RoomGrid.AddEntity(entity);

            Items.TriggerWired(WiredInteractionType.ENTER_ROOM, entity);

            await SendAsync(new EntitiesComposer(entity));
            await SendAsync(new EntityUpdateComposer(entity));
        }

        public async Task RemoveEntity(BaseEntity entity)
        {
            Entities.RemoveEntity(entity);
            RoomGrid.RemoveEntity(entity);
            Game.LeaveTeam(entity);

            await SendAsync(new EntityRemoveComposer(entity.Id));

			if (entity is PlayerEntity playerEntity)
			{
				if (playerEntity.Session == null ||
					!playerEntity.Session.Player.Online) return;

				playerEntity.Session.Entity = null;
				playerEntity.Session.CurrentRoom = null;
				await playerEntity.Session.SendPacketAsync(new HotelViewComposer());
			}
		}

		public async Task UpdateRoomGroup(IGroup group)
		{
			Group = group;
			foreach (BaseEntity entity in Entities.Entities)
			{
				if (entity is PlayerEntity playerEntity)
				{
					if (playerEntity.Session != null)
						await playerEntity.Session.SendPacketAsync(new GroupInfoComposer(group, playerEntity.Player, false));
				}
			}
		}

		public async Task SendAsync(IPacketComposer packet)
        {
            foreach (BaseEntity entity in Entities.Entities)
            {
                if (entity is PlayerEntity playerEntity)
                {
                    if (playerEntity.Session != null)
                        await playerEntity.Session.SendPacketAsync(packet);
                }
            }
        }

        public void Dispose()
        {
            // todo:
        }
    }
}
