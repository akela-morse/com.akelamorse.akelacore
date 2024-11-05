using Akela.Optimisations;
using UnityEngine;

namespace Akela.Behaviours
{
	[RequireComponent(typeof(CullingElement))]
	public abstract class OptimisedBehaviour : AbstractInitialisableBehaviour
	{
		#region Component Fields
		[SerializeField] OptimisationSettings optimisationSettings;
		#endregion

		private CullingElement _cullingElement;
		private float[] _timeBands;
		private float _lastUpdateTime;

		protected abstract void OptimisedUpdate();

		#region Component Messages
		private void Update()
		{
			if (optimisationSettings.stopExecutionWhenCulled && !_cullingElement.IsVisible)
				return;

			if (Time.time - _lastUpdateTime < _timeBands[_cullingElement.CurrentDistanceBand])
				return;

			_lastUpdateTime = Time.time;

			OptimisedUpdate();
		}
		#endregion

		#region Private Methods
		protected internal override void InitialiseBehaviour()
		{
			_cullingElement = GetComponent<CullingElement>();

			var system = _cullingElement.CullingSystem;

			_timeBands = new float[system.TopDistanceBand + 1];

			for (var i = 0; i < _timeBands.Length; ++i)
				_timeBands[i] = Mathf.Lerp(optimisationSettings.lowestTimeInterval, optimisationSettings.highestTimeInterval, (float)i / system.TopDistanceBand);
		}
		#endregion
	}
}