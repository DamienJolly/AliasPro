﻿using AliasPro.API.Navigator.Views;
using AliasPro.Room;
using AliasPro.Room.Models;
using System.Collections.Generic;

namespace AliasPro.Navigator.Models.Views
{
    internal class PromotionsCategory : ICategoryType
    {
        public override ICollection<IRoomData> Search(IRoomController roomController, uint categoryId, string searchCode, uint playerId)
        {
            ICollection<IRoomData> roomsToGo = new List<IRoomData>();
            return roomsToGo;
        }
    }
}
