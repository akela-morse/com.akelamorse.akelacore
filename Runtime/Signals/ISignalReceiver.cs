namespace Akela.Signals
{
	public interface ISignalReceiver
	{
		SignalType[] ListenFor {  get; }

		void OnSignalReceived(Signal signal);
	}
}
