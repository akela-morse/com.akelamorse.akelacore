using Akela.Globals;
using UnityEngine;

namespace Akela.Optimisations
{
	public class CullingSystem : MonoBehaviour
	{
		private const int MAX_ELEMENT_COUNT = 30;

		#region Component Fields
		[SerializeField] Var<Camera> _targetCamera;
		[Tooltip("Transform used as reference for distance calculations. Uses the camera's transform by default.")]
		[SerializeField] Var<Transform> _distanceReferenceOverride;
		[Space]
		[SerializeField] float[] _distanceBands;
		[SerializeField] float _maxiumCullingDistance;
		#endregion

		private readonly BoundingSphere[] _boundingSpheres = new BoundingSphere[MAX_ELEMENT_COUNT];
		private readonly ICullingElement[] _elements = new ICullingElement[MAX_ELEMENT_COUNT];
		private int _elementCount;
		private float[] _boundingDistances;
		private CullingGroup _cullingGroup;

		public int TopDistanceBand => _distanceBands.Length;

		public int RegisterElement(ICullingElement element, Vector4 shape)
		{
			var index = _elementCount++;

			_boundingSpheres[index] = new BoundingSphere(shape);
			_elements[index] = element;

			if (didAwake)
				_cullingGroup.SetBoundingSphereCount(_elementCount);

			return index;
		}

		public void UnregisterElement(int index)
		{
			if (_cullingGroup == null) // The system got destroyed before the element
				return;

			_cullingGroup.EraseSwapBack(index);
			CullingGroup.EraseSwapBack(index, _elements, ref _elementCount);

			_elements[index].IndexChanged(index);
		}

		public void UpdateSpherePosition(int elementId, Vector3 position)
		{
			_boundingSpheres[elementId].position = position;
		}

		#region Component Messages
		private void Awake()
		{
			ComputeBoundingDistances();

			_cullingGroup = new CullingGroup
			{
				targetCamera = _targetCamera ? Camera.main : _targetCamera,
				onStateChanged = OnStateChanged
			};

			_cullingGroup.SetBoundingSpheres(_boundingSpheres);
			_cullingGroup.SetBoundingSphereCount(_elementCount);

			_cullingGroup.SetBoundingDistances(_boundingDistances);
			_cullingGroup.SetDistanceReferencePoint(_distanceReferenceOverride ? _distanceReferenceOverride : _cullingGroup.targetCamera.transform);
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
			_cullingGroup = null;
		}

#if UNITY_EDITOR
		private void Reset()
		{
			_targetCamera = Camera.main;
			_distanceBands = new[] { .67f, .33f };
			_maxiumCullingDistance = 200f;
		}

		private void OnValidate()
		{
			if (_maxiumCullingDistance < 0f)
				_maxiumCullingDistance = 0f;
		}
#endif
		#endregion

		#region Private Methods
		private void ComputeBoundingDistances()
		{
			_boundingDistances = new float[_distanceBands.Length];

			var cumulatedPercent = 0f;

			for (var i = 0; i < _distanceBands.Length; ++i)
			{
				cumulatedPercent += _distanceBands[i];
				_boundingDistances[i] = cumulatedPercent * _maxiumCullingDistance;
			}
		}

		private void OnStateChanged(CullingGroupEvent e)
		{
			_elements[e.index].StateChanged(e);
		}
		#endregion
	}
}