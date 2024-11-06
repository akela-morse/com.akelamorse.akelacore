using UnityEngine;

namespace Akela.Behaviours
{
	public abstract class TickBehaviour : MonoBehaviour
	{
		public enum TickUpdateType
		{
			None,
			Update,
			LateUpdate,
			FixedUpdate,
			AnimatorMove
		}

		#region Component Fields
		[SerializeField] TickUpdateType _updateType = TickUpdateType.Update;
		#endregion

		protected abstract void Tick(float deltaTime);

		#region Component Messages
		private void Update()
		{
			if (_updateType != TickUpdateType.Update)
				return;

			Tick(Time.deltaTime);
		}

		private void LateUpdate()
		{
			if (_updateType != TickUpdateType.LateUpdate)
				return;

			Tick(Time.deltaTime);
		}

		private void FixedUpdate()
		{
			if (_updateType != TickUpdateType.FixedUpdate)
				return;

			Tick(Time.fixedDeltaTime);
		}

		private void OnAnimatorMove()
		{
			if (_updateType != TickUpdateType.AnimatorMove)
				return;

			Tick(Time.deltaTime);
		}
		#endregion
	}
}