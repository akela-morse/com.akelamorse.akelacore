namespace Akela.Behaviours
{
	public abstract class SingletonBehaviour<T> : AbstractInitialisableBehaviour where T : SingletonBehaviour<T>
	{
		public static T Main { get; private set; }

		protected internal sealed override void InitialiseBehaviour()
		{
			Main = (T)this;
		}
	}
}