namespace AliasPro.Chat
{
    using Configuration;
    using Database;

    internal class ChatDao : BaseDao
    {
        public ChatDao(IConfigurationController configurationController)
            : base(configurationController)
        {

        }
    }
}
