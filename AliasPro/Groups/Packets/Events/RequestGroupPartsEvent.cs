﻿using AliasPro.API.Groups;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Groups.Packets.Composers;
using System.Threading.Tasks;

namespace AliasPro.Groups.Packets.Events
{
	public class RequestGroupPartsEvent : IMessageEvent
	{
		public short Header => Incoming.RequestGroupPartsMessageEvent;

		private readonly IGroupController _groupController;

		public RequestGroupPartsEvent(
			IGroupController groupController)
		{
			_groupController = groupController;
		}

		public async Task RunAsync(
			ISession session,
			ClientMessage message)
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

