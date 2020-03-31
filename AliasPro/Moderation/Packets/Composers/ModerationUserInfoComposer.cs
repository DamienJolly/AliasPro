using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModerationUserInfoComposer : IMessageComposer
    {
        private readonly IPlayerData _playerData;
        
        public ModerationUserInfoComposer(IPlayerData playerData)
        {
            _playerData = playerData;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.ModerationUserInfoMessageComposer);
            message.WriteInt((int)_playerData.Id);
            message.WriteString(_playerData.Username);
            message.WriteString(_playerData.Figure);
            message.WriteInt(0); //account created
            message.WriteInt(0); //last online
            message.WriteBoolean(_playerData.Online);
            message.WriteInt(0); //cfh sent
            message.WriteInt(0); //cfh abuse
            message.WriteInt(0); //cfh warning
            message.WriteInt(0); //cfh bans
            message.WriteInt(0); //trade locks
            message.WriteString(string.Empty); //trading lock
            message.WriteString(string.Empty); //purchases
            message.WriteInt(0); //??
            message.WriteInt(0); //??
            message.WriteString("DamienJolly@hotmail.com"); //email
            message.WriteString(string.Empty); //??
            return message;
        }
    }
}
