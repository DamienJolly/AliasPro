using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Items.Packets.Composers
{
    public class BubbleAlertComposer : IPacketComposer
    {
		public static readonly string ADMIN_PERSISTENT = "admin.persistent";
		public static readonly string ADMIN_TRANSIENT = "admin.transient";
		public static readonly string BUILDERS_CLUB_MEMBERSHIP_EXPIRED = "builders_club.membership_expired";
		public static readonly string BUILDERS_CLUB_MEMBERSHIP_EXPIRES = "builders_club.membership_expires";
		public static readonly string BUILDERS_CLUB_MEMBERSHIP_EXTENDED = "builders_club.membership_extended";
		public static readonly string BUILDERS_CLUB_MEMBERSHIP_MADE = "builders_club.membership_made";
		public static readonly string BUILDERS_CLUB_MEMBERSHIP_RENEWED = "builders_club.membership_renewed";
		public static readonly string BUILDERS_CLUB_ROOM_LOCKED = "builders_club.room_locked";
		public static readonly string BUILDERS_CLUB_ROOM_UNLOCKED = "builders_club.room_unlocked";
		public static readonly string BUILDERS_CLUB_VISIT_DENIED_OWNER = "builders_club.visit_denied_for_owner";
		public static readonly string BUILDERS_CLUB_VISIT_DENIED_GUEST = "builders_club.visit_denied_for_visitor";
		public static readonly string CASINO_TOO_MANY_DICE_PLACEMENT = "casino.too_many_dice.placement";
		public static readonly string CASINO_TOO_MANY_DICE = "casino.too_many_dice";
		public static readonly string FLOORPLAN_EDITOR_ERROR = "floorplan_editor.error";
		public static readonly string FORUMS_DELIVERED = "forums.delivered";
		public static readonly string FORUMS_FORUM_SETTINGS_UPDATED = "forums.forum_settings_updated";
		public static readonly string FORUMS_MESSAGE_HIDDEN = "forums.message.hidden";
		public static readonly string FORUMS_MESSAGE_RESTORED = "forums.message.restored";
		public static readonly string FORUMS_THREAD_HIDDEN = "forums.thread.hidden";
		public static readonly string FORUMS_THREAD_LOCKED = "forums.thread.locked";
		public static readonly string FORUMS_THREAD_PINNED = "forums.thread.pinned";
		public static readonly string FORUMS_THREAD_RESTORED = "forums.thread.restored";
		public static readonly string FORUMS_THREAD_UNLOCKED = "forums.thread.unlocked";
		public static readonly string FORUMS_THREAD_UNPINNED = "forums.thread.unpinned";
		public static readonly string FURNITURE_PLACEMENT_ERROR = "furni_placement_error";
		public static readonly string GIFTING_VALENTINE = "gifting.valentine";
		public static readonly string NUX_POPUP = "nux.popup";
		public static readonly string PURCHASING_ROOM = "purchasing.room";
		public static readonly string RECEIVED_GIFT = "received.gift";
		public static readonly string RECEIVED_BADGE = "received.badge";
		public static readonly string FIGURESET_REDEEMED = "figureset.redeemed.success";
		public static readonly string FIGURESET_OWNED_ALREADY = "figureset.already.redeemed";

		private readonly string _errorkey;
		private readonly IDictionary<string, string> _keys;

        public BubbleAlertComposer(string errorkey)
        {
			_errorkey = errorkey;
			_keys = new Dictionary<string, string>();
		}

		public BubbleAlertComposer(string errorkey, IDictionary<string, string> keys)
		{
			_errorkey = errorkey;
			_keys = keys;
		}

		public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.BubbleAlertMessageComposer);
			message.WriteString(_errorkey);
			message.WriteInt(_keys.Count);
			foreach (var set in _keys)
			{
				message.WriteString(set.Key);
				message.WriteString(set.Value);
			}
			return message;
        }
    }
}
