using Akela.Optimisations;
using UnityEngine;

namespace Akela.Behaviours
{
	public abstract class OptimisedBehaviour : AbstractInitialisableBehaviour
	{
		#region Component Fields
		[SerializeField] OptimisationSettings _optimisationSettings;
		#endregion

		private CullingElement _cullingElement;
		private float[] _timeBands;
		private float _lastUpdateTime;

		protected abstract void OptimisedUpdate();

		#region Component Messages
		private void Update()
		{
			if (_optimisationSettings.stopExecutionWhenCulled && !_cullingElement.IsVisible)
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
			_cullingElement = _optimisationSettings.useCullingElementFrom switch
			{
				OptimisationSettings.CullingElementComponentSource.ThisGameObject => GetComponent<CullingElement>(),
				OptimisationSettings.CullingElementComponentSource.Parent => GetComponentInParent<CullingElement>(),
				OptimisationSettings.CullingElementComponentSource.Children => GetComponentInChildren<CullingElement>(),
				_ => GetComponent<CullingElement>(),
			};

#if UNITY_EDITOR
			if (_cullingElement == null)
			{
				Debug.LogError(string.Format("'{0}' on gameObject '{1}' did not find a CullingElement component from '{2}'",
					GetType().Name,
					gameObject.name,
					System.Enum.GetName(typeof(OptimisationSettings.CullingElementComponentSource), _optimisationSettings.useCullingElementFrom))
				);

				enabled = false;
				return;
			}
#endif

				var system = _cullingElement.CullingSystem;

			_timeBands = new float[system.TopDistanceBand + 1];

			for (var i = 0; i < _timeBands.Length; ++i)
				_timeBands[i] = Mathf.Lerp(_optimisationSettings.lowestTimeInterval, _optimisationSettings.highestTimeInterval, (float)i / system.TopDistanceBand);
		}
		#endregion
	}
}