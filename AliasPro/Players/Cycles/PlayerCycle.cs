using AliasPro.API.Players.Models;
using AliasPro.Players.Packets.Composers;
using System.Collections.Generic;

namespace AliasPro.Players.Cycles
{
    public class PlayerCycle
    {
        private readonly IPlayer _player;
        private readonly IDictionary<int, int> _tickTimers;

        public PlayerCycle(IPlayer player)
        {
            _player = player;
            _tickTimers = new Dictionary<int, int>();
        }

        public async void Cycle(IDictionary<int, ICurrencySetting> settings)
        {
            try
            {
                foreach (ICurrencySetting setting in settings.Values)
                {
                    if (!_tickTimers.ContainsKey(setting.Id))
                    {
                        _tickTimers.Add(setting.Id, 0);
                        continue;
                    }

                    if (_tickTimers[setting.Id] < setting.Time * 2)
                    {
                        _tickTimers[setting.Id]++;
                        continue;
                    }

                    if (setting.Id == 0) //credits
                    {
                        _player.Credits += setting.Amount;
                        await _player.Session.SendPacketAsync(new UserCreditsComposer(_player.Credits));
                    }
                    else
                    {
                        //todo: create currency for player
                        if (_player.Currency.TryGetCurrency(setting.Id, out IPlayerCurrency currency))
                        {
                            currency.Amount += setting.Amount;
                            await _player.Session.SendPacketAsync(new UserPointsComposer(currency.Amount, setting.Amount, currency.Type));
                        }
                    }

                    _tickTimers[setting.Id] = 0;
                }
            }
            catch { }
        }
    }
}
