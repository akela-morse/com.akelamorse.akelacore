#if AKELA_CINEMACHINE
using Unity.Cinemachine;
using UnityEngine;
using static Unity.Cinemachine.CinemachineFreeLookModifier;

namespace Akela.Tools
{
    public readonly struct CinemachineAxisControllers<T> where T : IInputAxisReader, new()
    {
        public readonly InputAxisControllerBase<T>.Controller orbitX;
        public readonly InputAxisControllerBase<T>.Controller orbitY;
        public readonly InputAxisControllerBase<T>.Controller orbitScale;

        public CinemachineAxisControllers(InputAxisControllerBase<T>.Controller orbitX, InputAxisControllerBase<T>.Controller orbitY, InputAxisControllerBase<T>.Controller orbitScale)
        {
            this.orbitX = orbitX;
            this.orbitY = orbitY;
            this.orbitScale = orbitScale;
        }
    }

    public static class CinemachineExtensions
    {
        public static CinemachineAxisControllers<T> GetControls<T>(this InputAxisControllerBase<T> axisController) where T : IInputAxisReader, new()
        {
            InputAxisControllerBase<T>.Controller orbitX = null, orbitY = null, orbitScale = null;

            foreach (var controller in axisController.Controllers)
            {
                if (controller.Name.Contains(" X"))
                    orbitX = controller;
                else if (controller.Name.Contains(" Y"))
                    orbitY = controller;
                else if (controller.Name.Contains(" Scale"))
                    orbitScale = controller;
            }

            return new(orbitX, orbitY, orbitScale);
        }

        public static T GetModifier<T>(this CinemachineFreeLookModifier freeLookModifier) where T : Modifier
        {
            foreach (var modifier in freeLookModifier.Modifiers)
            {
                if (modifier is T t)
                    return t;
            }

            return null;
        }

        public static CameraState.CustomBlendableItems.Item GetCustomBlendable<T>(this CameraState s) where T : Object
        {
            var count = s.GetNumCustomBlendables();

            for (var i = 0; i < count; ++i)
            {
                var item = s.GetCustomBlendable(i);

                if (item.Custom is T)
                    return item;
            }

            return default;
        }
    }
}
#endif