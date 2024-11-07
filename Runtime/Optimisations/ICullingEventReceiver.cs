namespace Akela.Optimisations
{
	public interface ICullingEventReceiver
	{
		void OnCullingElementVisible();
		void OnCullingElementInvisible();
		void OnDistanceBandChanges(int previousBand, int newBand);
	}
}
