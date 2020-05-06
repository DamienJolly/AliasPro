using AliasPro.API.Sessions.Models;
using System;

namespace AliasPro.Game.Habbos.Models
{
    internal class Habbo : HabboData
    {
        public ISession Session = null;

        public Habbo(ISession session, HabboData data)
            : base(data)
        {
            Session = session;
        }
    }


}
