using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Items.Packets.Events
{
    public class JukeBoxRemoveSoundTrackEvent : IMessageEvent
    {
        public short Header => Incoming.JukeBoxRemoveSoundTrackMessageEvent;

        private readonly IRoomController _roomController;
        private readonly IItemController _itemController;

        public JukeBoxRemoveSoundTrackEvent(
            IRoomController roomController,
            IItemController itemController)
        {
            _roomController = roomController;
            _itemController = itemController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IRoom room = session.CurrentRoom;
            if (room == null)
                return;

            if (room.OwnerId != session.Player.Id)
                return;

            int index = message.ReadInt();

            if (!room.Trax.TryGetSong(index, out IPlaylistSong song))
                return;

            IItem playerItem = await _itemController.GetPlayerItemByIdAsync((uint)song.ItemId);
            if (playerItem != null)
            {
                playerItem.PlayerId = session.Player.Id;
                playerItem.PlayerUsername = session.Player.Username;
                session.Player.Inventory.TryAddItem(playerItem);
                await _itemController.UpdatePlayerItemAsync(playerItem);

                await session.SendPacketAsync(new AddPlayerItemsComposer(1, (int)playerItem.Id));
                await session.SendPacketAsync(new InventoryRefreshComposer());
            }

            room.Trax.RemoveSong(index);
            await _roomController.RemoveRoomTraxAsync(room.Id, song.ItemId);

            await session.SendPacketAsync(new JukeBoxPlayListComposer(room.Trax.Songs, room.Trax.maxSongs));
            await session.SendPacketAsync(new JukeBoxMySongsComposer(room.Trax.AvailablePlayerSongs(session.Player.Inventory.Items)));
        }
    }
}
