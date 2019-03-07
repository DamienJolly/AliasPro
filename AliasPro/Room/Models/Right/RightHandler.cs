using System.Threading.Tasks;
using System.Collections.Generic;

namespace AliasPro.Room.Models.Right
{
    using Player.Models;
    using Sessions;
    using Packets.Outgoing;

    public class RightHandler
    {
        private readonly IRoom _room;
        private readonly IDictionary<uint, string> _rights;

        public RightHandler(IRoom room)
        {
            _room = room;
            _rights = new Dictionary<uint, string>();
        }

        public async Task ReloadRights(ISession session)
        {
            RightLevel flatCtrl = RightLevel.NONE;

            if (IsOwner(session.Player.Id))
            {
                await session.SendPacketAsync(new RoomOwnerComposer());
                flatCtrl = RightLevel.OWNER;
            }
            //todo: group rights
            else if (HasRights(session.Player.Id))
            {
                flatCtrl = RightLevel.RIGHTS;
            }

            await session.SendPacketAsync(new RoomRightsComposer((int)flatCtrl));
            
            session.Entity.Actions.AddStatus("flatctrl", (int)flatCtrl + "");
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
            _room.RoomData.OwnerId == playerId;
        
        public bool HasRights(uint playerId) =>
            IsOwner(playerId) || _rights.ContainsKey(playerId);
    }
}
