using AliasPro.API.Items.Models;

namespace AliasPro.API.Items.Interaction
{
    public interface IWiredInteractor
    {
        IWiredData WiredData { get; set; }
        bool OnTrigger(params object[] args);
        void OnCycle();
    }
}
