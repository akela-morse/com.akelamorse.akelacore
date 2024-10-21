using UnityEngine;

namespace Akela.Optimisations
{
	public class CullingSystem : MonoBehaviour
	{
		#region Component Fields
		[SerializeField] Camera _targetCamera;
		[SerializeField] Transform _distanceReferenceOverride;
		[Header("Shape")]
		[SerializeField] Bounds _bounds = new(Vector3.zero, Vector3.one);
		[SerializeField, Range(1, 8)] int _subdivisions = 1;
		#endregion

		private CullingGroup _cullingGroup;

		#region Component Messages
		private void Awake()
		{
			_cullingGroup = new CullingGroup
			{
				targetCamera = _targetCamera ? Camera.main : _targetCamera,
				onStateChanged = OnStateChanged
			};

			_cullingGroup.SetDistanceReferencePoint(_distanceReferenceOverride ? _distanceReferenceOverride : _targetCamera.transform);
		}

		private void OnEnable()
		{
			_cullingGroup.enabled = true;
		}

		private void OnDisable()
		{
			_cullingGroup.enabled = false;
		}

		private void OnDestroy()
		{
			_cullingGroup.Dispose();
		}

#if UNITY_EDITOR
		private void Reset()
		{
			_targetCamera = Camera.main;
		}
#endif
		#endregion

		#region Private Methods
		private void OnStateChanged(CullingGroupEvent e)
		{
		}
		#endregion
	}
}
