//using SOL.Utilities;
//using System;
//using UltEvents;
//using UnityEngine;

//namespace SOL.Triggers
//{
//	public class FlipFlopTrigger : MonoBehaviour, ITrigger
//	{
//		public event Action OnActivate;
//		public event Action OnDeactivate { add { } remove { } } // unused in this context

//		#region Component Fields
//		public bool startFlipped;

//		[SerializeField] UltEvent _onFlipped;

//		[SerializeField] UltEvent _onUnflipped;
//		#endregion

//		private bool _active;
//		private bool _isFlipped;

//		public bool IsFlipped => _isFlipped;
//		public bool IsActive() => _active;

//		public void Flip()
//		{
//			_isFlipped = !_isFlipped;

//			if (_isFlipped)
//				_onFlipped.Invoke();
//			else
//				_onUnflipped.Invoke();

//			_active = true;
//			OnActivate?.Invoke();
//		}

//		public void ForceState(bool flipped)
//		{
//			_isFlipped = flipped;

//			if (_isFlipped)
//				_onFlipped.Invoke();
//			else
//				_onUnflipped.Invoke();
//		}

//		#region Component Fields
//		private void Start()
//		{
//			if (startFlipped)
//				ForceState(true);
//		}
//		#endregion
//	}
//}
