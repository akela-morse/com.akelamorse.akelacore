namespace Akela.Tools
{
	public static class StringHelpers
	{
		public static string NumberToAlphabet(int number, bool isCaps)
		{
			var c = (char)((isCaps ? 65 : 97) + (number - 1));
			return c.ToString();
		}
	}
}