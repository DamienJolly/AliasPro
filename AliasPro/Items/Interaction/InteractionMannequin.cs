﻿using AliasPro.API.Figure;
using AliasPro.API.Items.Models;
using AliasPro.API.Messenger;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Figure.Packets.Composers;
using AliasPro.Players.Types;
using AliasPro.Rooms.Entities;
using System.Collections.Generic;

namespace AliasPro.Items.Interaction
{
    public class InteractionMannequin : ItemInteraction
    {
        public string Gender = "m";
        public string Figure = "";
        public string OutfitName = "My Look";

        public string ExtraData =>
            Gender + ":" + Figure + ":" + OutfitName;

        public InteractionMannequin(IItem item)
            : base(item)
        {
            string[] data = Item.ExtraData.Split(":");
            if (data.Length == 3)
            {
                Gender = data[0].ToLower();
                Figure = data[1];
                OutfitName = data[2];
            }
        }

        public override void ComposeExtraData(ServerMessage message)
        {
			message.WriteInt(1);
			message.WriteInt(3);
            message.WriteString("GENDER");
            message.WriteString(Gender);
            message.WriteString("FIGURE");
            message.WriteString(Figure);
            message.WriteString("OUTFIT_NAME");
            message.WriteString(OutfitName);
        }
        
        public async override void OnUserInteract(BaseEntity entity, int state)
        {
            if (!(entity is PlayerEntity playerEntity))
                return;

            IList<string> parts = new List<string>();
            foreach (string playerPart in playerEntity.Player.Figure.Split('.'))
            {
                string type = playerPart.Split("-")[0];
                bool found = false;

                foreach (string mannPart in Figure.Split('.'))
                {
                    if (mannPart.Contains(type))
                    {
                        found = true;
                        parts.Add(mannPart);
                    }
                }

                if (!found)
                    parts.Add(playerPart);
            }

            string figure = string.Join(".", parts);
            PlayerGender gender = Gender == "m" ? PlayerGender.MALE : PlayerGender.FEMALE;

            if (!Program.GetService<IFigureController>().ValidateFigure(figure, gender))
                return;

            playerEntity.Player.Figure = playerEntity.Figure = figure;
            playerEntity.Player.Gender = playerEntity.Gender = gender;
            await playerEntity.Session.SendPacketAsync(new UpdateFigureComposer(playerEntity.Figure, playerEntity.Gender));
            await Item.CurrentRoom.SendPacketAsync(new UpdateEntityDataComposer(playerEntity));

            if (playerEntity.Player.Messenger != null)
                await Program.GetService<IMessengerController>().UpdateStatusAsync(playerEntity.Player, playerEntity.Player.Messenger.Friends);
        }
    }
}
