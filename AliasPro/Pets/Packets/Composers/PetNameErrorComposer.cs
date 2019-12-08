using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Pets.Packets.Composers
{
    public class PetNameErrorComposer : IPacketComposer
    {
		public static int NAME_OK = 0;
		public static int NAME_TO_LONG = 1;
		public static int NAME_TO_SHORT = 2;
		public static int FORBIDDEN_CHAR = 3;

		private readonly int _error;
		private readonly string _data;

		public PetNameErrorComposer(int error, string data)
        {
			_error = error;
			_data = data;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.PetNameErrorMessageComposer);
            message.WriteInt(_error);
            message.WriteString(_data);
            return message;
        }
    }
}
