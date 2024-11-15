namespace Akela.Signals
{
    public interface ISignalReceiver
    {
        Signal[] ListenFor {  get; }

        void OnSignalReceived(Signal signal);
    }
}
