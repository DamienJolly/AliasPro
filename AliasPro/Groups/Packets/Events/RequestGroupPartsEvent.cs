using AliasPro.API.Groups;
using AliasPro.API.Groups.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Groups.Packets.Composers;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Groups.Packets.Events
{
	public class RequestGroupPartsEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.RequestGroupPartsMessageEvent;

		private readonly IGroupController _groupController;

		public RequestGroupPartsEvent(
			IGroupController groupController)
		{
			_groupController = groupController;
		}

		public async void HandleAsync(
			ISession session,
			IClientPacket clientPacket)
		{
			await session.SendPacketAsync(new GroupPartsComposer(
				_groupController.GetBases,
				_groupController.GetSymbols,
				_groupController.GetBaseColours,
				_groupController.GetSymbolColours,
				_groupController.GetBackgroundColours));
		}
	}
}

