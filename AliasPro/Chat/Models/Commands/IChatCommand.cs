using System.Threading.Tasks;

namespace AliasPro.Chat.Models.Commands
{
    using Sessions;

    interface IChatCommand
    {
        string Name { get; }
        string Description { get; }
        Task Handle(ISession session, string[] args);
    }
}
