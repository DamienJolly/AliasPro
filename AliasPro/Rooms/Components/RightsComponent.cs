using AliasPro.API.Groups.Models;
using AliasPro.API.Groups.Types;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Rooms.Packets.Composers;
using AliasPro.Rooms.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Rooms.Components
{
    public class RightsComponent
    {
        private readonly IRoom _room;
        private readonly IDictionary<uint, string> _rights;

        public RightsComponent(IRoom room, IDictionary<uint, string> rights)
        {
            _room = room;
            _rights = rights;
        }

        public async Task ReloadRights(ISession session)
        {
            RightLevel flatCtrl = RightLevel.NONE;

            if (IsOwner(session.Player.Id))
            {
                await session.SendPacketAsync(new RoomOwnerComposer());
                flatCtrl = RightLevel.OWNER;
            }
			else if (_room.Group != null)
			{
				if (_room.Group.IsAdmin((int)session.Player.Id))
				{
					flatCtrl = RightLevel.GROUP_ADMIN;
				}
				/*else if (_room.Group.IsMember((int)session.Player.Id) && _room.Group.Rights)
				{
					flatCtrl = RightLevel.GROUP_RIGHTS;
				}*/
			}
			else if (HasRights(session.Player.Id))
            {
                flatCtrl = RightLevel.RIGHTS;
            }

			session.Entity.Actions.AddStatus("flatctrl", (int)flatCtrl + "");
			await session.SendPacketAsync(new RoomRightsComposer((int)flatCtrl));
        }

        public void GiveRights(uint playerId, string playerUsername)
        {
            if (!_rights.ContainsKey(playerId))
            {
                _rights.Add(playerId, playerUsername);
            }
        }

        public void RemoveRights(uint playerId) =>
            _rights.Remove(playerId);

        public bool IsOwner(uint playerId) =>
            _room.OwnerId == playerId;

		public bool HasRights(uint playerId)
		{
			if (IsOwner(playerId)) return true;
			else if (_room.Group != null)
			{
				if (_room.Group.IsAdmin((int)playerId)) return true;
				// else if (_room.Group.IsMember((int)session.Player.Id) && _room.Group.Rights) return true;
			}
			else if (_rights.ContainsKey(playerId)) return true;

			return false;
		}
    }
}
