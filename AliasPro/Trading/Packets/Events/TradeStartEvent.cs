﻿using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Rooms.Entities;
using AliasPro.API.Trading;
using AliasPro.API.Trading.Models;
using AliasPro.Trading.Packets.Composers;

namespace AliasPro.Trading.Packets.Events
{
	public class TradeStartEvent : IAsyncPacket
	{
		public short Header { get; } = Incoming.TradeStartMessageEvent;

		private readonly ITradingController _tradingController;

		public TradeStartEvent(
			ITradingController tradingController)
		{
			_tradingController = tradingController;
		}

		public async void HandleAsync(
			ISession session,
			IClientPacket clientPacket)
		{
			IRoom room = session.CurrentRoom;
			if (room == null) return;

			int userId = clientPacket.ReadInt();
			if (userId == session.Entity.Id) return;

			//if (room.TradeState == RoomTradeState.FORBIDDEN ||
			//	(room.RoomData.TradeState == RoomTradeState.OWNER && session.Player.Id != room.RoomData.OwnerId))
			{
				//await session.SendPacketAsync(new TradeStartFailComposer(TradeStartFailComposer.ROOM_TRADING_NOT_ALLOWED));
				//return;
			}

			if (!room.Entities.TryGetEntityById(userId, out BaseEntity entity))
				return;

			if (!(entity is PlayerEntity playerEntity))
				return;

			if (session.Entity.Actions.HasStatus("trd"))
			{
				await session.SendPacketAsync(new TradeStartFailComposer(TradeStartFailComposer.YOU_ALREADY_TRADING));
				return;
			}

			//if (!session.Player.AllowTrading)
			{
				//await session.SendPacketAsync(new TradeStartFailComposer(TradeStartFailComposer.YOU_TRADING_OFF));
				//return;
			}

			if (entity.Actions.HasStatus("trd"))
			{
				await session.SendPacketAsync(new TradeStartFailComposer(TradeStartFailComposer.TARGET_ALREADY_TRADING, playerEntity.Name));
				return;
			}

			//if (!playerEntity.Player.AllowTrading)
			{
				//await session.SendPacketAsync(new TradeStartFailComposer(TradeStartFailComposer.TARGET_TRADING_OFF, playerEntity.Name));
				//return;
			}

			ITrade trade = _tradingController.StartTrade(session.Entity, playerEntity);
			if (trade == null) return;

			await trade.SendAsync(new TradeStartComposer(trade));
		}
	}
}
