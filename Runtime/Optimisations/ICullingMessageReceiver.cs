namespace Akela.Optimisations
{
    public interface ICullingMessageReceiver
    {
        void OnCullingElementVisible();
        void OnCullingElementInvisible();
        void OnDistanceBandChanges(int previousBand, int newBand);
    }
}
