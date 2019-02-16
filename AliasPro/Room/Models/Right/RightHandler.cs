using System.Threading.Tasks;
using System.Collections.Generic;

namespace AliasPro.Room.Models.Right
{
    using Sessions;
    using Packets.Outgoing;

    public class RightHandler
    {
        private readonly IRoom _room;
        private readonly IDictionary<uint, int> _rights;

        public RightHandler(IRoom room)
        {
            _room = room;
            _rights = new Dictionary<uint, int>();
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

            session.Entity.ActiveStatuses.Remove("flatctrl");
            session.Entity.ActiveStatuses.Add("flatctrl", (int)flatCtrl + "");
        }

        public bool IsOwner(uint playerId) =>
            _room.RoomData.OwnerId == playerId;
        
        public bool HasRights(uint playerId) =>
            IsOwner(playerId) || _rights.ContainsKey(playerId);
    }
}
