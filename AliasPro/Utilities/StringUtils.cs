using System;
using System.Collections.Generic;

namespace AliasPro.Utilities
{
    public static class StringUtils
	{
		private static readonly IList<char> _allowedchars = new List<char>(new[]
			{
				'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
				'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
				'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '-', '.'
			});

		public static bool IsAlphanumeric(string inputStr)
		{
			inputStr = inputStr.ToLower();

			if (string.IsNullOrEmpty(inputStr))
				return false;

			for (int i = 0; i < inputStr.Length; i++)
			{
				if (!IsValid(inputStr[i]))
					return false;
			}

			return true;
		}

		private static bool IsValid(char character) =>
			_allowedchars.Contains(character);

		public static T ToEnum<T>(this string value) => 
			(T)Enum.Parse(typeof(T), value, true);
	}
}
