#if AKELA_ANIMATION_RIGGING
using System;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

namespace Akela.Animation
{
    [BurstCompile]
    public struct NoiseJob : IWeightedAnimationJob
    {
        public ReadWriteTransformHandle constrained;
        public FloatProperty noiseAmplitude;
        public FloatProperty noiseFrequency;
        public Vector3Property positionDrive;
        public Vector3Property rotationDrive;
        public IntProperty seed;

        public FloatProperty jobWeight { get; set; }

        private float t;

        public readonly void ProcessRootMotion(AnimationStream stream) { }

        public void ProcessAnimation(AnimationStream stream)
        {
            var w = jobWeight.Get(stream);
            t += stream.deltaTime;

            if (w > 0f)
            {
                var a = noiseAmplitude.Get(stream);
                var f = noiseFrequency.Get(stream);
                var dp = positionDrive.Get(stream);
                var dr = rotationDrive.Get(stream);
                var rp = constrained.GetLocalPosition(stream);
                var rr = constrained.GetLocalRotation(stream);
                var s = seed.Get(stream);

                var p = a * w * new float3(
                    noise.snoise(new float2(t * f, s * 1.0f)) * dp.x,
                    noise.snoise(new float2(t * f, s * 1.1f)) * dp.y,
                    noise.snoise(new float2(t * f, s * 1.2f)) * dp.z
                );

                var r = a * w * new float3(
                    noise.snoise(new float2(t * f, s * 1.3f)) * dr.x,
                    noise.snoise(new float2(t * f, s * 1.4f)) * dr.y,
                    noise.snoise(new float2(t * f, s * 1.5f)) * dr.z
                );

                constrained.SetLocalPosition(stream, rp + (Vector3)p);
                constrained.SetLocalRotation(stream, rr * quaternion.Euler(r));
            }
        }
    }

    [Serializable]
    public struct NoiseData : IAnimationJobData
    {
        public Transform constrainedObject;
        [Space]
        [SyncSceneToStream] public float noiseAmplitude;
        [SyncSceneToStream] public float noiseFrequency;
        [Space]
        [SyncSceneToStream] public Vector3 positionDrive;
        [SyncSceneToStream] public Vector3 rotationDrive;

        [HideInInspector] public int seed;

        public readonly bool IsValid()
        {
            return constrainedObject != null;
        }

        public void SetDefaultValues()
        {
            constrainedObject = null;
            noiseAmplitude = 0f;
            noiseFrequency = 0f;
            positionDrive = Vector3.zero;
            rotationDrive = Vector3.zero;
        }
    }

    public class NoiseBinder : AnimationJobBinder<NoiseJob, NoiseData>
    {
        public override NoiseJob Create(Animator animator, ref NoiseData data, Component component)
        {
            data.seed = component.gameObject.GetInstanceID();

            return new NoiseJob()
            {
                constrained = ReadWriteTransformHandle.Bind(animator, data.constrainedObject),
                noiseAmplitude = FloatProperty.Bind(animator, component, ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(data.noiseAmplitude))),
                noiseFrequency = FloatProperty.Bind(animator, component, ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(data.noiseFrequency))),
                positionDrive = Vector3Property.Bind(animator, component, ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(data.positionDrive))),
                rotationDrive = Vector3Property.Bind(animator, component, ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(data.rotationDrive))),
                seed = IntProperty.Bind(animator, component, ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(data.seed)))
            };
        }

        public override void Destroy(NoiseJob job) { }
    }

    [DisallowMultipleComponent]
    public class NoiseConstraint : RigConstraint<NoiseJob, NoiseData, NoiseBinder> { }
}
#endif