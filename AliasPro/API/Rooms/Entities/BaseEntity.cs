using AliasPro.API.Rooms.Models;
using AliasPro.Network.Protocol;
using AliasPro.Players.Types;
using AliasPro.Rooms.Models;
using AliasPro.Rooms.Models.Entities;
using AliasPro.Rooms.Types;
using System.Collections.Generic;

namespace AliasPro.API.Rooms.Entities
{
    public abstract class BaseEntity
    {
        protected BaseEntity(int id, int x, int y, int rotation, string name, string figure, PlayerGender gender, string motto)
        {
            Id = id;
            BodyRotation = rotation;
            HeadRotation = rotation;
            Position = new RoomPosition(x, y, 0);
            NextPosition = new RoomPosition(x, y, 0);
            Name = name;
            Figure = figure;
            Gender = gender;
            Motto = motto;

            Actions = new EntityAction();
        }

        public void SetHandItem(int handItemId)
        {
            HandItemTimer = 0;
            HandItemId = handItemId;

            if (handItemId != 0)
                HandItemTimer = 240;
        }

        public int Id { get; set; }
        public int BodyRotation { get; set; }
        public int HeadRotation { get; set; }
        public IRoomPosition Position { get; set; }
        public IRoomPosition NextPosition { get; set; }
        public IList<IRoomPosition> PathToWalk { get; set; }
        public string Name { get; set; }
        public string Figure { get; set; }
        public PlayerGender Gender { get; set; }
        public string Motto { get; set; }

        public EntityAction Actions;

        public int DanceId { get; set; } = 0;
        public bool IsIdle { get; set; } = false;
        public bool IsSitting { get; set; } = false;

        public int DirOffsetTimer = 0;
        public int IdleTimer = 0;

        public int HandItemId = 0;
        public int HandItemTimer = 0;

        public GameTeamType Team = GameTeamType.NONE;

        public abstract void Compose(ServerPacket serverPacket);
    }
}
