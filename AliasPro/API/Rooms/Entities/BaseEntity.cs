using AliasPro.API.Rooms.Models;
using AliasPro.Network.Protocol;
using AliasPro.Players.Types;
using AliasPro.Rooms.Models;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Rooms.Types;
using Pathfinding;
using Pathfinding.Models;
using Pathfinding.Types;
using System.Collections.Generic;

namespace AliasPro.API.Rooms.Entities
{
    public abstract class BaseEntity
    {
        protected BaseEntity(int id, int x, int y, int rotation, IRoom room, string name, string figure, PlayerGender gender, string motto, int score)
        {
            Id = id;
            Room = room;
            BodyRotation = rotation;
            HeadRotation = rotation;
            Position = new RoomPosition(x, y, 0);
            NextPosition = new RoomPosition(x, y, 0);
            Name = name;
            Figure = figure;
            Gender = gender;
            Motto = motto;
            Score = score;

            Actions = new EntityAction();
        }

        public void FindPath(int targetX, int targetY)
        {
            PathToWalk = Pathfinder.FindPath(
                Room.RoomGrid,
                new Position(Position.X, Position.Y),
                new Position(targetX, targetY),
                DiagonalMovement.ONE_WALKABLE,
                this);
        }

        public async void Unidle()
        {
            IdleTimer = 0;
            IsIdle = false;
            await Room.SendAsync(new UserSleepComposer(this));
        }

        public void SetRotation(int rotation)
        {
            HeadRotation = rotation;
            BodyRotation = rotation;
        }

        public void SetHandItem(int handItemId)
        {
            HandItemTimer = 0;
            HandItemId = handItemId;

            if (handItemId != 0)
                HandItemTimer = 240;
        }

        public int Id { get; set; }
        public IRoom Room { get; set; }
        public int BodyRotation { get; set; }
        public int HeadRotation { get; set; }
        public IRoomPosition Position { get; set; }
        public IRoomPosition NextPosition { get; set; }
        public IList<Position> PathToWalk { get; set; }
        public string Name { get; set; }
        public string Figure { get; set; }
        public PlayerGender Gender { get; set; }
        public string Motto { get; set; }
        public int Score { get; set; }
        
        public IEntityAction Actions;

        public int DanceId { get; set; } = 0;
        public bool IsIdle { get; set; } = false;
        public bool IsSitting { get; set; } = false;
        
        public int DirOffsetTimer = 0;
        public int IdleTimer = 0;

        public int HandItemId = 0;
        public int HandItemTimer = 0;

        public GameTeamType Team = GameTeamType.NONE;

        public abstract void Cycle();
        public abstract void Compose(ServerPacket serverPacket);
    }
}
