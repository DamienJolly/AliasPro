using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Items.Packets.Events
{
    public class SongDataEvent : IMessageEvent
    {
        public short Header => Incoming.SongDataMessageEvent;

		private readonly IItemController _itemController;

		public SongDataEvent(IItemController itemController)
		{
			_itemController = itemController;
		}

		public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            IList<ISongData> songs = new List<ISongData>();

            int count = message.ReadInt();
            for (int i = 0; i < count; i++)
            {
                int songId = message.ReadInt();

                if (!_itemController.TryGetSongDataById(songId, out ISongData song))
                    continue;

                songs.Add(song);
            }

            await session.SendPacketAsync(new SongDataComposer(songs));
        }
    }
}
