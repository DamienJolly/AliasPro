using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using AliasPro.Room.Models;

namespace AliasPro.Room.Packets.Composers
{
    public class RoomSettingsComposer : IPacketComposer
    {
        private readonly IRoomData _roomData;

        public RoomSettingsComposer(IRoomData roomData)
        {
            _roomData = roomData;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomSettingsMessageComposer);
            message.WriteInt(_roomData.Id);
            message.WriteString(_roomData.Name);
            message.WriteString(_roomData.Description);
            message.WriteInt(_roomData.DoorState);
            message.WriteInt(_roomData.CategoryId);
            message.WriteInt(_roomData.MaxUsers);
            message.WriteInt(50); //dunno?
            message.WriteInt(0); //tags
            message.WriteInt(_roomData.TradeType);
            message.WriteInt(_roomData.Settings.AllowPets ? 1 : 0);
            message.WriteInt(_roomData.Settings.AllowPetsEat ? 1 : 0);
            message.WriteInt(_roomData.Settings.RoomBlocking ? 1 : 0);
            message.WriteInt(_roomData.Settings.HideWalls ? 1 : 0);
            message.WriteInt(_roomData.Settings.WallThickness);
            message.WriteInt(_roomData.Settings.FloorThickness);
            message.WriteInt(_roomData.Settings.ChatMode);
            message.WriteInt(_roomData.Settings.ChatSize);
            message.WriteInt(_roomData.Settings.ChatSpeed);
            message.WriteInt(_roomData.Settings.ChatDistance);
            message.WriteInt(_roomData.Settings.ChatFlood);
            message.WriteBoolean(false); //dunno?
            message.WriteInt(_roomData.Settings.WhoMutes);
            message.WriteInt(_roomData.Settings.WhoKicks);
            message.WriteInt(_roomData.Settings.WhoBans);
            return message;
        }
    }
}
