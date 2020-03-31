using AliasPro.API.Players.Models;
using AliasPro.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace AliasPro.Players.Components
{
    public class SanctionComponent
	{
        private readonly IList<IPlayerSanction> _sanctions;

        public SanctionComponent(
			IList<IPlayerSanction> sanctions)
        {
			_sanctions = sanctions;
        }

		public void AddSanction(IPlayerSanction sanction) =>
			_sanctions.Add(sanction);

		public bool GetCurrentSanction(out IPlayerSanction sanction)
		{
			sanction = _sanctions.Where(x => x.ExpireTime >= (int)UnixTimestamp.Now)
						.OrderByDescending(x => x.ExpireTime)
						.FirstOrDefault();
			return sanction != null;
		}
	}
}
