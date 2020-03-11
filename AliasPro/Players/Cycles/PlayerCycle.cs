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
                _player.CheckLastOnline();

                foreach (ICurrencySetting setting in settings.Values)
                {
                    if (setting.Time == 0)
                        continue;

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

                    IPlayerCurrency currency = 
                        await _player.GetPlayerCurrency(setting.Id);

                    if (currency != null)
                    {
                        if (setting.CyclesPerDay == 0 || setting.CyclesPerDay > currency.Cycles)
                        {
                            if (setting.Maximum == 0 || currency.Amount != setting.Maximum)
                            {
                                currency.Amount += setting.Amount;
                                if (setting.Maximum != 0 && currency.Amount > setting.Maximum)
                                    currency.Amount = setting.Maximum;

                                System.Console.WriteLine(setting.Id + ": success; amount: " + currency.Amount);
                                currency.Cycles++;

                                System.Console.WriteLine("uhm");
                                if (setting.Id == -1)
                                {
                                    await _player.Session.SendPacketAsync(new UserCreditsComposer(currency.Amount));
                                }
                                else
                                {
                                    await _player.Session.SendPacketAsync(new UserPointsComposer(currency.Amount, setting.Amount, currency.Type));
                                }
                            }
                        }
                    }

                    _tickTimers[setting.Id] = 0;
                }
            }
            catch { }
        }
    }
}
