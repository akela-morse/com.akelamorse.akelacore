using System;

namespace Akela.Tools
{
	public static class StringExtensions
	{
		public static string ReplaceAt(this string input, int index, char newChar)
		{
			if (input == null)
				return null;

			char[] chars = input.ToCharArray();
			chars[index] = newChar;
			return new string(chars);
		}

		public static string ReplaceLast(this string input, string find, string replace)
		{
			int place = input.LastIndexOf(find);

			if (place == -1)
				return input;

			string result = input.Remove(place, find.Length).Insert(place, replace);

			return result;
		}

		public static int GetNthIndex(this string s, char t, int n)
		{
			int count = 0;
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == t)
				{
					count++;
					if (count == n)
					{
						return i;
					}
				}
			}

			return -1;
		}

		public static int GetNextIndex(this string s, char t, int index)
		{
			return s.Substring(index).IndexOf(t) + index;
		}

		public static int GetPreviousIndex(this string s, char t, int index)
		{
			return s.Substring(0, index + 1).LastIndexOf(t);
		}

		public static string FirstCharToUpper(this string input)
		{
			switch (input)
			{
				case null: throw new ArgumentNullException();
				case "": throw new ArgumentException();
				default: return input[0].ToString().ToUpper() + input.Substring(1);
			}
		}

		public static int DistanceTo(this string s, string t)
		{
			int n = s.Length;
			int m = t.Length;
			int[,] d = new int[n + 1, m + 1];

			// Step 1
			if (n == 0)
			{
				return m;
			}

			if (m == 0)
			{
				return n;
			}

			// Step 2
			for (int i = 0; i <= n; d[i, 0] = i++) { }

			for (int j = 0; j <= m; d[0, j] = j++) { }

			// Step 3
			for (int i = 1; i <= n; i++)
			{
				//Step 4
				for (int j = 1; j <= m; j++)
				{
					// Step 5
					int cost = t[j - 1] == s[i - 1] ? 0 : 1;

					// Step 6
					d[i, j] = Math.Min(
						Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
						d[i - 1, j - 1] + cost);
				}
			}
			// Step 7
			return d[n, m];
		}
	}
}