namespace Akela.Optimisations
{
	internal interface ICullingMessageReceiver
	{
		void OnSystemBecameCulled();
		void OnSystemBecameVisible();
		void OnSystemDistanceLodChanges(int distanceBand, in int distanceBandCount);
	}
}
