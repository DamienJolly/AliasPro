using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Pets.Packets.Composers
{
    public class PetNameErrorComposer : IMessageComposer
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

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.PetNameErrorMessageComposer);
            message.WriteInt(_error);
            message.WriteString(_data);
            return message;
        }
    }
}
