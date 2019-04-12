using AliasPro.Players.Models;

namespace AliasPro.API.Figure
{
    public interface IFigureController
    {
        bool ValidateFigure(string figure, PlayerGender gender);
    }
}
