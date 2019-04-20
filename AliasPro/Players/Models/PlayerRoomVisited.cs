using AliasPro.API.Database;
using AliasPro.API.Players.Models;
using AliasPro.Network.Protocol;
using System.Data.Common;

namespace AliasPro.Players.Models
{
    internal class PlayerRoomVisited : IPlayerRoomVisited
    {
        internal PlayerRoomVisited(DbDataReader reader)
        {
            RoomId = reader.ReadData<int>("room_id");
            RoomName = reader.ReadData<string>("name");
            Timestamp = reader.ReadData<int>("timestamp");
        }

        public void Compose(ServerPacket message)
        {
            message.WriteInt(RoomId);
            message.WriteString(RoomName);
            //todo:
            message.WriteInt(0); //mins ago
            message.WriteInt(0); //hours ago
        }

        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int Timestamp { get; set; }
    }
}
