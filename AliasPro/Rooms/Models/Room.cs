﻿using AliasPro.API.Items.Models;
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
using System.Collections.Generic;
using AliasPro.Rooms.Types;
using AliasPro.Items.Interaction;
using AliasPro.Communication.Messages;
using AliasPro.API.Players.Models;
using AliasPro.Players.Types;
using AliasPro.Utilities;
using System.Linq;
using AliasPro.Items.Packets.Composers;
using AliasPro.API.Items;
using AliasPro.API.Players;

namespace AliasPro.Rooms.Models
{
    internal class Room : RoomData, IRoom, IDisposable
    {
        public EntitiesComponent Entities { get; set; }
        public ItemsComponent Items { get; set; }
        public RightsComponent Rights { get; set; }
        public GameComponent Game { get; set; }
        public MuteComponent Mute { get; set; }
        public BanComponent Bans { get; set; }
        public TraxComponent Trax { get; set; }
        public MoodlightComponent Moodlight { get; set; }
        public RoomGrid RoomGrid { get; set; }
        public RoomCycle RoomCycle { get; set; }
        public IList<string> WordFilter { get; set; }
        public int IdleTimer { get; set; } = 0;
        public int RollerSpeed { get; set; } = 2;
        public int RollerCycle { get; set; } = 0;
		public bool Loaded { get; set; } = false;
		public bool Muted { get; set; } = false;

        internal Room(IRoomData roomData)
            : base(roomData)
        {
            Game = new GameComponent();
        }

        public async void Cycle()
        {
            try
            {
                foreach (BaseEntity entity in Entities.Entities)
                    entity.RoomEntityCycle.Cycle();

                foreach (IItem item in Items.Items)
                    item.Interaction.OnCycle();

                if (Entities.Entities.Count > 0)
                    await SendPacketAsync(new EntityUpdateComposer(Entities.Entities));
            }
            catch { }
        }

		public async void OnChat(string text, int colour, BaseEntity entity, RoomChatType chatType)
        {
            ICollection<BaseEntity> targetEntities = new List<BaseEntity>();
            string targetName = string.Empty;

            if (colour == 1 || colour == -1 || colour == 2)
            {
                colour = 0;
            }

            if (text.Length > 100)
            {
                text = text.Substring(0, 100);
            }

            if (entity is PlayerEntity player)
            {
                if (!Rights.HasRights(player.Player.Id))
                {
                    if (Muted) return;

                    if (Mute.PlayerMuted((int)player.Player.Id))
                    {
                        await player.Session.SendPacketAsync(new MutedWhisperComposer(Mute.MutedTimeLeft((int)player.Player.Id)));
                        return;
                    }
                }

                if (player.Player.Sanction.GetCurrentSanction(out IPlayerSanction sanction) &&
                    sanction.Type == SanctionType.MUTE)
                {
                    await player.Session.SendPacketAsync(new MutedWhisperComposer(sanction.ExpireTime - (int)UnixTimestamp.Now));
                    return;
                }
            }

            foreach (string word in WordFilter)
                text = text.Replace(word, "bobba");

            if (chatType == RoomChatType.WHISPER)
            {
                targetName = text.Split(' ')[0];
                text = text.Substring(targetName.Length + 1);

                if (!Entities.TryGetPlayerEntityByName(targetName, out BaseEntity targetEntity))
                    return;

                targetEntities.Add(targetEntity);
                targetEntities.Add(entity);
            }
            else if (entity.Room.RoomGrid.TryGetRoomTile(entity.Position.X, entity.Position.Y, out IRoomTile tile) && 
                tile.TopItem != null && 
                tile.TopItem.Interaction is InteractionTent interaction)
            {
                targetEntities = interaction.TentEntities;
            }
            else
            {
                targetEntities = Entities.Entities;
            }

            Items.TriggerWired(WiredInteractionType.SAY_SOMETHING, entity, text);

            foreach (BaseEntity targetEntity in targetEntities)
            {
                string message = text;
                if (chatType == RoomChatType.WHISPER && targetEntity == entity)
                {
                    message = "[Whisper to " + targetName + "] " + text;
                }

                if (targetEntity != entity)
				{
                    int newDir = targetEntity.BodyRotation == 0 ? 8 : targetEntity.Position.CalculateDirection(entity.Position.X, entity.Position.Y);

					if (Math.Abs(newDir - targetEntity.BodyRotation) <= 1)
						targetEntity.SetRotation(newDir, true);

					targetEntity.DirOffsetTimer = 0;
				}

				if (targetEntity is PlayerEntity playerEntity)
				{
                    if (entity is PlayerEntity targetPlayer && !playerEntity.Player.Ignore.TryGetIgnoredUser((int)targetPlayer.Player.Id))
                        await playerEntity.Session.SendPacketAsync(new AvatarChatComposer(entity.Id, message, 0, colour, chatType));
				}
			}
        }

		public async Task AddEntity(BaseEntity entity)
        {
            Entities.AddEntity(entity);
            RoomGrid.AddEntity(entity);

			entity.OnEntityJoin();
			Items.TriggerWired(WiredInteractionType.ENTER_ROOM, entity);

            await SendPacketAsync(new EntitiesComposer(entity));
            await SendPacketAsync(new EntityUpdateComposer(entity));
        }

		public async Task RemoveEntity(BaseEntity entity, bool notifyUser)
        {
			entity.OnEntityLeave();
			Entities.RemoveEntity(entity);
            RoomGrid.RemoveEntity(entity);

			await SendPacketAsync(new EntityRemoveComposer(entity.Id));

			if (entity is PlayerEntity playerEntity)
			{
				if (playerEntity.Session == null) return;

                if (playerEntity.GamePlayer != null)
                    playerEntity.GamePlayer.Game.LeaveTeam(playerEntity);

                if (playerEntity.Trade != null)
                    await playerEntity.Trade.EndTrade(false, playerEntity.Player.Id);

                playerEntity.Session.Entity = null;
				playerEntity.Session.CurrentRoom = null;

				if (notifyUser)
					await playerEntity.Session.SendPacketAsync(new HotelViewComposer());
			}
		}

		public async Task UpdateRoomGroup(IGroup group)
		{
			Group = group;
			foreach (BaseEntity entity in Entities.Entities.ToList())
			{
				if (entity is PlayerEntity playerEntity)
                {
                    if (playerEntity.Session == null)
                        continue;

                    await playerEntity.Session.SendPacketAsync(new GroupInfoComposer(group, playerEntity.Player, false));
                    await Rights.ReloadRights(playerEntity.Session);
                }
            }
		}

        public async Task SendPacketAsync(IMessageComposer packet)
        {
            foreach (BaseEntity entity in Entities.Entities.ToList())
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
			Entities = null;
			Items = null;
			Rights = null;
			Game = null;
			RoomCycle = null;
			Entities = null;
		}
    }
}
