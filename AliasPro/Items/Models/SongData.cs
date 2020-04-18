using AliasPro.API.Database;
using AliasPro.API.Items.Models;
using System.Data.Common;

namespace AliasPro.Items.Models
{
    internal class SongData : ISongData
    {
        internal SongData(DbDataReader reader)
        {
            Id = reader.ReadData<int>("id");
            Code = reader.ReadData<string>("code");
            Name = reader.ReadData<string>("name");
            Author = reader.ReadData<string>("author");
            Track = reader.ReadData<string>("track");
            Length = reader.ReadData<int>("length");
        }

        public int Id { get; }
        public string Code { get; }
        public string Name { get; }
        public string Author { get; }
        public string Track { get; }
        public int Length { get; }
    }
}
