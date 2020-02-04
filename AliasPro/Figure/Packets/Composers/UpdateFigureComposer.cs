using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Types;

namespace AliasPro.Figure.Packets.Composers
{
    public class UpdateFigureComposer : IMessageComposer
    {
        private readonly string _figure;
        private readonly PlayerGender _gender;

        public UpdateFigureComposer(string figure, PlayerGender gender)
        {
            _figure = figure;
            _gender = gender;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.UpdateFigureMessageComposer);
            message.WriteString(_figure);
            message.WriteString(_gender == PlayerGender.MALE ? "m" : "f");
            return message;
        }
    }
}
