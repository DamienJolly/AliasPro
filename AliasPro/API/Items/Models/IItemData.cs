﻿using AliasPro.Items.Types;

namespace AliasPro.API.Items.Models
{
    public interface IItemData
    {
        uint Id { get; }
        uint SpriteId { get; }
        string Name { get; }
        int Length { get; }
        int Width { get; }
        double Height { get; }
        string ExtraData { get; }
        string Type { get; }
        int Modes { get; }
        bool CanWalk { get; set; }
        bool CanStack { get; }
        bool AllowRecycle { get; }
        bool AllowTrade { get; }
        bool AllowInventoryStack { get; }
        bool AllowMarketplace { get; }
        string Interaction { get; }

        ItemInteractionType InteractionType { get; }
        WiredInteractionType WiredInteractionType { get; }

        bool IsWired { get; }
    }
}
