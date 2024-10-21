using UnityEngine;

namespace Akela.Behaviours
{
	public abstract class SingletonUIBehaviour<T> : AbstractInitialisableBehaviour where T : SingletonUIBehaviour<T>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Field hiding")]
		protected new RectTransform transform => (RectTransform)base.transform;

		public static T Main { get; private set; }

		protected internal sealed override void InitialiseBehaviour()
		{
			Main = (T)this;
		}
	}
}