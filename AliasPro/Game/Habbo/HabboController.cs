using System.Collections.Generic;

namespace AliasPro.Game.Habbo
{
	public class HabboController
	{
		private readonly HabboDao dao;

		private readonly Dictionary<uint, Habbo> habbos;
		private readonly Dictionary<string, uint> habboUsernames;

		public HabboController(HabboDao dao)
		{
			this.dao = dao;

			habbos = new Dictionary<int, Habbo>();
		}
	}
}
