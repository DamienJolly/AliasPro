using AliasPro.API.Items;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Items.Packets.Composers;
using AliasPro.Items.Types;
using AliasPro.Rooms.Models;
using AliasPro.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace AliasPro.Rooms.Components
{
    public class TraxComponent
    {
        private readonly IRoom _room;
        private readonly IList<int> _loadedSongs;
        private readonly IDictionary<int, IPlaylistSong> _playlistSongs;

        public bool CurrentlyPlaying = false;
        //todo: make this a configurable setting
        public int maxSongs = 20;

        private int _startTime = 0;
        private int _currentlyIndex = -1;
        private IPlaylistSong _currentSong = null;

        public TraxComponent(IRoom room, IDictionary<int, IItem> musicDiscs)
        {
            _room = room;
            _loadedSongs = new List<int>();
            _playlistSongs = new Dictionary<int, IPlaylistSong>();

            foreach (IItem musicDisc in musicDiscs.Values)
            {
                int.TryParse(musicDisc.ItemData.ExtraData, out int songId);
                if (!Program.GetService<IItemController>().TryGetSongDataById(songId, out ISongData song))
                {
                    //todo: return item
                    return;
                }

                if (!TryAddSong((int)musicDisc.Id, song))
                {
                    //todo: return item
                    return;
                }
            }
        }

        public async void LoadCurrentSong(ISession session)
        {
            if (!CurrentlyPlaying)
                return;

            await session.SendPacketAsync(new JukeBoxNowPlayingComposer(_currentSong.SongData.Id, _currentlyIndex, TimeElapsed * 1000));
        }

        public async void Play(int index = 0)
        {
            if (_playlistSongs.Count <= 0)
            {
                Stop();
                return;
            }

            if (!TryGetSong(index, out IPlaylistSong song))
            {
                Stop();
                return;
            }


            CurrentlyPlaying = true;
            _currentSong = song;
            _currentlyIndex = index;
            _startTime = (int)UnixTimestamp.Now;

            await _room.SendPacketAsync(new JukeBoxNowPlayingComposer(_currentSong.SongData.Id, _currentlyIndex));
        }

        public async void Stop()
        {
            CurrentlyPlaying = false;
            _currentSong = null;
            _currentlyIndex = -1;
            _startTime = 0;

            await _room.SendPacketAsync(new JukeBoxNowPlayingComposer());
        }

        public void Cycle()
        {
            if (CurrentlyPlaying)
            {
                if (TimeElapsed >= _currentSong.SongData.Length)
                {
                    Play((_currentlyIndex + 1) % _playlistSongs.Count);
                }
            }
        }


        public IDictionary<int, int> AvailablePlayerSongs(ICollection<IItem> playerItems)
        {
            IDictionary<int, int> musicDiscs = new Dictionary<int, int>();
            foreach (IItem item in playerItems)
            {
                if (item.ItemData.InteractionType != ItemInteractionType.MUSICDISC)
                    continue;

                int.TryParse(item.ItemData.ExtraData, out int songId);
                if (!Program.GetService<IItemController>().TryGetSongDataById(songId, out ISongData song))
                    continue;

                if(_loadedSongs.Contains(song.Id))
                    continue;

                musicDiscs.TryAdd(song.Id, (int)item.Id);
            }
            return musicDiscs;
        }

        public bool TryGetSong(int index, out IPlaylistSong song) =>
            _playlistSongs.TryGetValue(index, out song);

        public bool TryAddSong(int itemId, ISongData song)
        {
            if (_loadedSongs.Contains(song.Id))
                return false;

            if (!_playlistSongs.TryAdd(_playlistSongs.Count, new PlaylistSong(itemId, song)))
                return false;

            _loadedSongs.Add(song.Id);
            return true;
        }

        public void RemoveSong(int index)
        {
            if (!TryGetSong(index, out IPlaylistSong song))
                return;

            _playlistSongs.Remove(index);
            _loadedSongs.Remove(song.SongData.Id);
            RepairPlaylist();
        }

        public async void RepairPlaylist()
        {
            _loadedSongs.Clear();
            IList<IPlaylistSong> songs = new List<IPlaylistSong>();

            lock (_playlistSongs)
            {
                songs = _playlistSongs.Values.ToList();
                _playlistSongs.Clear();
            }

            foreach (IPlaylistSong song in songs)
            {
                TryAddSong(song.ItemId, song.SongData);
            }

            if (CurrentlyPlaying)
            {
                if (_playlistSongs.Count <= 0)
                {
                    Stop();
                    return;
                }

                foreach (var playlistSong in _playlistSongs)
                {
                    if (playlistSong.Value.SongData.Id == _currentSong.SongData.Id)
                    {
                        if (_currentlyIndex != playlistSong.Key)
                        {
                            _currentlyIndex = playlistSong.Key;
                            await _room.SendPacketAsync(new JukeBoxNowPlayingComposer(_currentSong.SongData.Id, _currentlyIndex, TimeElapsed * 1000));
                        }
                        return;
                    }
                }

                Play((_currentlyIndex + 1) % _playlistSongs.Count);
            }
        }

        public ICollection<IPlaylistSong> Songs =>
            _playlistSongs.Values;

        public int TimeElapsed =>
            (int)UnixTimestamp.Now - _startTime;
    }
}
