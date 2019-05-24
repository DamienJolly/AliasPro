using AliasPro.API.Groups.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Groups.Packets.Composers
{
	public class GroupPartsComposer : IPacketComposer
	{
		private readonly ICollection<IGroupBadgePart> _bases;
		private readonly ICollection<IGroupBadgePart> _symbols;
		private readonly ICollection<IGroupBadgePart> _baseColours;
		private readonly ICollection<IGroupBadgePart> _symbolColours;
		private readonly ICollection<IGroupBadgePart> _backgroundColours;

		public GroupPartsComposer(
			ICollection<IGroupBadgePart> bases,
			ICollection<IGroupBadgePart> symbols,
			ICollection<IGroupBadgePart> baseColours,
			ICollection<IGroupBadgePart> symbolColours,
			ICollection<IGroupBadgePart> backgroundColours)
		{
			_bases = bases;
			_symbols = symbols;
			_baseColours = baseColours;
			_symbolColours = symbolColours;
			_backgroundColours = backgroundColours;
		}

		public ServerPacket Compose()
		{
			ServerPacket message = new ServerPacket(Outgoing.GroupPartsMessageComposer);
			message.WriteInt(_bases.Count);
			foreach (IGroupBadgePart part in _bases)
			{
				message.WriteInt(part.Id);
				message.WriteString(part.AssetOne);
				message.WriteString(part.AssetTwo);
			}

			message.WriteInt(_symbols.Count);
			foreach (IGroupBadgePart part in _symbols)
			{
				message.WriteInt(part.Id);
				message.WriteString(part.AssetOne);
				message.WriteString(part.AssetTwo);
			}

			message.WriteInt(_baseColours.Count);
			foreach (IGroupBadgePart part in _baseColours)
			{
				message.WriteInt(part.Id);
				message.WriteString(part.AssetOne);
			}

			message.WriteInt(_symbolColours.Count);
			foreach (IGroupBadgePart part in _symbolColours)
			{
				message.WriteInt(part.Id);
				message.WriteString(part.AssetOne);
			}

			message.WriteInt(_backgroundColours.Count);
			foreach (IGroupBadgePart part in _backgroundColours)
			{
				message.WriteInt(part.Id);
				message.WriteString(part.AssetOne);
			}
			return message;
		}
	}
}
