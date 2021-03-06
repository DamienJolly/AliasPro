﻿using AliasPro.Players.Types;
using System.Collections.Generic;

namespace AliasPro.Figure.Components
{
    internal class FigureComponent
    {
        public FigureComponent()
        {
            //todo: figure - all this shit
        }

        public bool Validate(string figure, PlayerGender gender)
        {
            ICollection<string> usedParts = new List<string>();
            string[] sets = figure.Split('.');

            foreach (string set in sets)
            {
                string[] parts = set.Split('-');

                if (parts.Length < 2 || parts.Length > 4) return false;

                if (!CheckPart(parts[0]) || !CheckType(parts[1]))
                    return false;

                if (parts.Length >= 3 && !CheckColourTwo(parts[2])) return false;

                if (parts.Length >= 4 && !CheckColourTwo(parts[3])) return false;

                if (!usedParts.Contains(parts[0]))
                    usedParts.Add(parts[0]);
                else
                    return false;
            }

            if (!usedParts.Contains("hd") || !usedParts.Contains("lg")) return false;

            if (gender == PlayerGender.FEMALE && !usedParts.Contains("ch")) return false;

            return true;
        }

        private bool CheckPart(string i)
        {
            switch (i)
            {
                case "hd":
                case "he":
                case "ha":
                case "hr":
                case "cp":
                case "cc":
                case "ca":
                case "ch":
                case "lg":
                case "wa":
                case "fa":
                case "ea":
                case "sh":
                    return true;
                default:
                    return false;
            }
        }

        private bool CheckType(string i)
        {
            if (int.TryParse(i, out int type))
            {
                if (type < 0)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        private bool CheckColourOne(string i)
        {
            if (int.TryParse(i, out int colour))
            {
                if (colour < 0)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        private bool CheckColourTwo(string i)
        {
            if (int.TryParse(i, out int colour))
            {
                if (colour < 0)
                {
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
