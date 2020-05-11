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

		public static T ToEnum<T>(this string value, T defaultValue) 
			where T : struct
		{
			if (string.IsNullOrEmpty(value))
				return defaultValue;

			return Enum.TryParse(value, true, out T result) ? result : defaultValue;
		}

		public static string MergeParams(string[] args, int start, int end)
		{
			IList<string> parts = new List<string>();
			for (int i = start; i < end; i++)
			{
				parts.Add(args[i]);
			}
			return string.Join(" ", parts);
		}

		public static string Left(this string input, int length)
		{
			string result = input;
			if (input != null && input.Length > length)
			{
				result = input.Substring(0, length);
			}
			return result;
		}
	}
}
