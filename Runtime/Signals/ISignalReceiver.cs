namespace Akela.Signals
{
    public interface ISignalReceiver
    {
        string[] ListenFor {  get; }

        void OnSignalReceived(Signal signal);
    }
}