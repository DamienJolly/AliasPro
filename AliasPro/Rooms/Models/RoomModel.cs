﻿using AliasPro.API.Database;
using AliasPro.API.Rooms.Models;
using AliasPro.Rooms.Utils;
using System;
using System.Data.Common;
using System.Text;

namespace AliasPro.Rooms.Models
{
    internal class RoomModel : IRoomModel
    {
        private double[,] floorHeightMap;
        private bool[,] tileStateMap;

        internal RoomModel(DbDataReader reader)
        {
            Id = reader.ReadData<string>("id");
            HeightMap = reader.ReadData<string>("heightMap");
            DoorX = reader.ReadData<int>("door_x");
            DoorY = reader.ReadData<int>("door_Y");
            DoorDir = reader.ReadData<int>("door_dir");
            InitializeHeightMap();
        }

        internal RoomModel(string id, string map, int doorX, int doorY, int doorDir)
        {
            Id = id;
            HeightMap = map;
            DoorX = doorX;
            DoorY = doorY;
            DoorDir = doorDir;
            InitializeHeightMap();
        }

        public int MapSizeX { get; set; }
        public int MapSizeY { get; set; }
        public int DoorDir { get; set; }
        public int DoorX { get; set; }
        public int DoorY { get; set; }
        public double DoorZ { get; set; }
        public string Id { get; set; }
        public string HeightMap { get; set; }
        public string RelativeHeightMap { get; set; }
        public bool IsCustom => 
            Id.StartsWith("model_bc_");

        public double GetHeight(int x, int y) =>
            floorHeightMap[x, y];

        public bool GetTileState(int x, int y) =>
            tileStateMap[x, y];

        public void InitializeHeightMap()
        {
            ParseHeightMap();
            ParseRelativeMap();
        }

        private void ParseHeightMap()
        {
            string[] splitHeightMap = HeightMap.Split('\r');
            MapSizeX = splitHeightMap[0].Length;
            MapSizeY = splitHeightMap.Length;
            floorHeightMap = new double[MapSizeX, MapSizeY];
            tileStateMap = new bool[MapSizeX, MapSizeY];

            for (int y = 0; y < MapSizeY; y++)
            {
                char[] line = splitHeightMap[y].Replace("\r", "").Replace("\n", "").ToCharArray();

                int x = 0;
                foreach (char square in line)
                {
                    if (x > MapSizeX)
                    {
                        throw new FormatException($"Invalid room model! Model Id: {Id}");
                    }

                    if (square == 'x')
                    {
                        //Square is blocked!
                        tileStateMap[x, y] = false;
                    }
                    else
                    {
                        tileStateMap[x, y] = true;
                        floorHeightMap[x, y] = square.Parse();
                    }

                    x++;
                }
            }

            DoorZ = floorHeightMap[DoorX, DoorY];
        }

        private void ParseRelativeMap()
        {
            StringBuilder relativeMap = new StringBuilder();
            for (int y = 0; y < MapSizeY; y++)
            {
                for (int x = 0; x < MapSizeX; x++)
                {
                    if (x == DoorX && y == DoorY)
                    {
                        relativeMap.Append(DoorZ > 9 ? ((char)(87 + DoorZ)).ToString() : DoorZ.ToString());
                        continue;
                    }

                    if (tileStateMap[x, y])
                    {
                        double Height = floorHeightMap[x, y];
                        string Val = Height > 9 ? ((char)(87 + Height)).ToString() : Height.ToString();
                        relativeMap.Append(Val);
                    }
                    else
                    {
                        relativeMap.Append("x");
                        continue;
                    }


                }

                relativeMap.Append((char)13);
            }

            RelativeHeightMap = relativeMap.ToString();
        }
    }
}
