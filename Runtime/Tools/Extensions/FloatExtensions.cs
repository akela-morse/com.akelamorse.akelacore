namespace Akela.Tools
{
	public static class FloatExtensions
	{
		public static float Angle360To180(this float angle)
		{
			return angle > 180f ? angle - 360f : angle;
		}
	}
}