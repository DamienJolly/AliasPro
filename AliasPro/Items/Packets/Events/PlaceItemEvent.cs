using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Items.Packets.Events
{
    public class PlaceItemEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.PlaceItemMessageEvent;

        private readonly IItemController _itemController;
        public PlaceItemEvent(IItemController itemController)
        {
            _itemController = itemController;
        }
        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            string rawData = clientPacket.ReadString();
            string[] data = rawData.Split(' ');

            if (!uint.TryParse(data[0], out uint itemId)) return;

            IRoom room = session.CurrentRoom;
            
            if (!room.Rights.HasRights(session.Player.Id)) return;

            if (session.Player.Inventory.TryGetItem(itemId, out IItem item))
            {
                if (item.ItemData.Type == "s")
                {
                    int x = int.Parse(data[1]);
                    int y = int.Parse(data[2]);
                    int rot = int.Parse(data[3]);

                    if (!room.RoomGrid.TryGetRoomTile(x, y, out IRoomTile roomTile)) return;

                    if (!room.RoomGrid.CanStackAt(x, y, item)) return;

                    item.Position.X = x;
                    item.Position.Y = y;
                    item.Position.Z = item.ItemData.InteractionType == Types.ItemInteractionType.STACK_TOOL ? roomTile.Position.Z : roomTile.Height;
                    item.Rotation = rot;
                    room.RoomGrid.AddItem(item);

                    await room.SendAsync(new ObjectAddComposer(item));
                }
                else
                {
                    if (data.Length < 4) return;

                    item.ExtraData = data[1] + " " + data[2] + " " + data[3];
                    await session.SendPacketAsync(new AddWallItemComposer(item));
                }
                
                item.RoomId = room.Id;
                item.CurrentRoom = room;
				room.Items.AddItem(item);
				item.Interaction.OnPlaceItem();
				session.Player.Inventory.RemoveItem(item.Id);
                await _itemController.UpdatePlayerItemAsync(item);

                await session.SendPacketAsync(new RemovePlayerItemComposer(item.Id));
            }
        }
    }
}
