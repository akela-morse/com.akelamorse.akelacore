using System.Diagnostics.CodeAnalysis;
using Akela.Behaviours;
using Akela.Tools;
using UnityEngine;

namespace Akela.ExtendedPhysics
{
    [GenerateHashForEveryField]
    [Icon("Packages/com.akelamorse.akelacore/Editor/EditorResources/TorusCollider Icon.png")]
    [AddComponentMenu("Physics/Torus Collider", 101)]
    public partial class TorusCollider : CustomCollider<CapsuleCollider>
    {
        #region Component Fields
        public Vector3 center;
        public float radius = .75f;
        public float thickness = .25f;
        public Axis direction = Axis.Y;

        [SerializeField, Range(3, 64)] int _resolution = 8;
        #endregion

        #region Component Messages
#if UNITY_EDITOR
        [SuppressMessage("ReSharper","ConditionIsAlwaysTrueOrFalse")]
        public void OnValidate()
        {
            if (_resolution < 3)
                _resolution = 3;

            if (_resolution > 64)
                _resolution = 64;

            if (radius < 0f)
                radius = 0f;

            if (thickness < 0f)
                thickness = 0f;
        }
#endif
        #endregion

        #region Private Methods
        protected override bool ShouldRebuild()
        {
            return _subColliders == null || _subColliders.Length != _resolution;
        }

        protected override void Build()
        {
            if (_subColliders != null)
            {
                for (var i = 0; i < _subColliders.Length; ++i)
                {
                    if (_subColliders[i])
                        _subColliders[i].gameObject.PlaymodeAgnosticDestroy();
                }
            }

            _subColliders = new CapsuleCollider[_resolution];

            for (var i = 0; i < _resolution; ++i)
            {
                var newObject = new GameObject("Collider Instance " + (i + 1));
                newObject.transform.SetParent(transform);
                newObject.layer = gameObject.layer;

                _subColliders[i] = newObject.AddComponent<CapsuleCollider>();
            }
        }

        protected override void RefreshSubCollider(int index)
        {
            var theta = 2f * Mathf.PI / _resolution;
            var length = 2f * Mathf.PI * radius / _resolution;
            var offset = transform.TransformPoint(center);

            var origin = GetNthPoint(index, theta) + offset;
            var rot = theta * index * Mathf.Rad2Deg * direction.ToVector3();

            if (direction == Axis.Y)
                rot *= -1f;

            _subColliders[index].transform.position = origin;
            _subColliders[index].transform.localEulerAngles = rot;
            _subColliders[index].height = length + thickness * 2f;
            _subColliders[index].radius = thickness;
            _subColliders[index].direction = direction switch
            {
                Axis.X => 2,
                Axis.Y => 2,
                Axis.Z => 1,
                _ => 0
            };
        }

        private Vector3 GetNthPoint(int n, float theta)
        {
            var x = radius * Mathf.Cos(theta * n);
            var y = radius * Mathf.Sin(theta * n);

            var xOffset = direction switch
            {
                Axis.X => transform.up * x,
                _ => transform.right * x
            };

            var yOffset = direction switch
            {
                Axis.Z => transform.up * y,
                _ => transform.forward * y
            };

            var finalPoint = xOffset + yOffset;
            finalPoint.Scale(transform.lossyScale);

            return finalPoint;
        }
        #endregion
    }
}