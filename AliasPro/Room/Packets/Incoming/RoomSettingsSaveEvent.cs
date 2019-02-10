using System.Threading.Tasks;

namespace AliasPro.Room.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;
    using Models;

    public class RoomSettingsSaveEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RoomSettingsSaveMessageEvent;

        private readonly IRoomController _roomController;

        public RoomSettingsSaveEvent(IRoomController roomController)
        {
            _roomController = roomController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            uint roomId = (uint)clientPacket.ReadInt();
            IRoom room = await _roomController.GetRoomByIdAsync(roomId);

            if (room == null) return;

            if (!room.RightHandler.IsOwner(session.Player.Id)) return;

            string name = clientPacket.ReadString();
            if (name.Length > 60)
                name.Substring(0, 60);

            if (name.Length <= 0)
            {
                await session.SendPacketAsync(new RoomEditSettingsErrorComposer(room.RoomData.Id, RoomEditSettingsErrorComposer.ROOM_NAME_MISSING));
            }
            else
            {
                room.RoomData.Name = name;
            }

            room.RoomData.Description = clientPacket.ReadString();
            
            int doorState = clientPacket.ReadInt();
            if (doorState < 0 || doorState > 3) doorState = 0;

            string password = clientPacket.ReadString();

            if (doorState == 2 && password.Length <= 0)
            {
                await session.SendPacketAsync(new RoomEditSettingsErrorComposer(room.RoomData.Id, RoomEditSettingsErrorComposer.PASSWORD_REQUIRED));
            }
            else
            {
                room.RoomData.DoorState = doorState;
                room.RoomData.Password = password;
            }

            int maxUsers = clientPacket.ReadInt();
            if ((maxUsers % 5) != 0 || 
                maxUsers < 10 || 
                maxUsers > 50)
                maxUsers = 10;

            room.RoomData.MaxUsers = maxUsers;

            //todo: category check
            room.RoomData.CategoryId = clientPacket.ReadInt();

            //todo: tags
            int amount = clientPacket.ReadInt();
            for (int i = 0; i < amount && i < 2; i++)
            {
                string tag = clientPacket.ReadString();
                if (tag.Length > 15)
                {
                    await session.SendPacketAsync(new RoomEditSettingsErrorComposer(room.RoomData.Id, RoomEditSettingsErrorComposer.TAGS_TOO_LONG));
                    continue;
                }

                //add
            }
            
            int tradeType = clientPacket.ReadInt();
            if (tradeType < 0 || tradeType > 2) tradeType = 0;

            room.RoomData.TradeType = tradeType;

            room.RoomData.Settings.AllowPets = clientPacket.ReadBool();
            room.RoomData.Settings.AllowPetsEat = clientPacket.ReadBool();
            room.RoomData.Settings.RoomBlocking = clientPacket.ReadBool();
            room.RoomData.Settings.HideWalls = clientPacket.ReadBool();

            int wallThickness = clientPacket.ReadInt();
            if (wallThickness < -2 || wallThickness > 1) wallThickness = 0;

            int floorThickness = clientPacket.ReadInt();
            if (floorThickness < -2 || floorThickness > 1) floorThickness = 0;

            int whoMutes = clientPacket.ReadInt();
            if (whoMutes < 0 || whoMutes > 1) whoMutes = 0;

            int whoKicks = clientPacket.ReadInt();
            if (whoKicks < 0 || whoKicks > 1) whoKicks = 0;

            int whoBans = clientPacket.ReadInt();
            if (whoBans < 0 || whoBans > 1) whoBans = 0;

            room.RoomData.Settings.WallThickness = wallThickness;
            room.RoomData.Settings.FloorThickness = floorThickness;
            room.RoomData.Settings.WhoMutes = whoMutes;
            room.RoomData.Settings.WhoKicks = whoKicks;
            room.RoomData.Settings.WhoBans = whoBans;

            int chatMode = clientPacket.ReadInt();
            if (chatMode < 0 || chatMode > 1) chatMode = 0;

            int chatSize = clientPacket.ReadInt();
            if (chatSize < 0 || chatSize > 2) chatSize = 1;

            int chatSpeed = clientPacket.ReadInt();
            if (chatSpeed < 0 || chatSpeed > 2) chatSpeed = 1;

            int chatDistance = clientPacket.ReadInt();
            if (chatDistance < 0) chatDistance = 1;
            else if (chatDistance > 100) chatDistance = 14;

            int chatFlood = clientPacket.ReadInt();
            if (chatFlood < 0 || chatFlood > 2) chatFlood = 1;

            room.RoomData.Settings.ChatMode = chatMode;
            room.RoomData.Settings.ChatSize = chatSize;
            room.RoomData.Settings.ChatSpeed = chatSpeed;
            room.RoomData.Settings.ChatDistance = chatDistance;
            room.RoomData.Settings.ChatFlood = chatFlood;

            await room.SendAsync(new RoomVisualizationSettingsComposer(room.RoomData.Settings));
            await room.SendAsync(new RoomChatSettingsComposer(room.RoomData.Settings));
            await room.SendAsync(new RoomSettingsUpdatedComposer(room.RoomData.Id));

            await session.SendPacketAsync(new RoomSettingsSavedComposer(room.RoomData.Id));
        }
    }
}
