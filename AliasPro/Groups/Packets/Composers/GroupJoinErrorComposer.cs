﻿using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Groups.Packets.Composers
{
	public class GroupJoinErrorComposer : IMessageComposer
	{
		public static readonly int GROUP_FULL = 0;
		public static readonly int GROUP_LIMIT_EXCEED = 1;
		public static readonly int GROUP_CLOSED = 2;
		public static readonly int GROUP_NOT_ACCEPT_REQUESTS = 3;
		public static readonly int NON_HC_LIMIT_REACHED = 4;
		public static readonly int MEMBER_FAIL_JOIN_LIMIT_EXCEED_NON_HC = 5;
		public static readonly int MEMBER_FAIL_JOIN_LIMIT_EXCEED_HC = 6;

		private readonly int _code;

		public GroupJoinErrorComposer(
			int code)
		{
			_code = code;
		}

		public ServerMessage Compose()
		{
			ServerMessage message = new ServerMessage(Outgoing.GroupJoinErrorMessageComposer);
			message.WriteInt(_code);
			return message;
		}
	}
}
