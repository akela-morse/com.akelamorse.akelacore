using UnityEditor;
using UnityEngine;

namespace AkelaEditor
{
    internal static class AnimationTools
    {
        [MenuItem("CONTEXT/" + nameof(AnimationClip) + "/Generate Root Motion Curves", validate = true)]
        private static bool AddRootMotionCurveValidate(MenuCommand command)
        {
            if (!EditorUtility.IsPersistent(command.context) || (command.context.hideFlags & HideFlags.NotEditable) != 0)
                return false;

            var clip = (AnimationClip)command.context;

            if (clip.isHumanMotion || clip.hasMotionCurves || !clip.hasGenericRootTransform)
                return false;

            return true;
        }

        [MenuItem("CONTEXT/" + nameof(AnimationClip) + "/Generate Root Motion Curves")]
        private static void AddRootMotionCurve(MenuCommand command)
        {
            var clip = (AnimationClip)command.context;

            var bindings = AnimationUtility.GetCurveBindings(clip);

            AnimationCurve tX = null, tY = null, tZ = null, qX = null, qY = null, qZ = null, qW = null;

            for (var i = 0; i < bindings.Length; ++i)
            {
                if (!string.IsNullOrEmpty(bindings[i].path) || bindings[i].type != typeof(Transform))
                    continue;

                switch (bindings[i].propertyName)
                {
                    case "m_LocalPosition.x":
                        tX = AnimationUtility.GetEditorCurve(clip, bindings[i]);
                        AnimationUtility.SetEditorCurve(clip, bindings[i], null);
                        break;

                    case "m_LocalPosition.y":
                        tY = AnimationUtility.GetEditorCurve(clip, bindings[i]);
                        AnimationUtility.SetEditorCurve(clip, bindings[i], null);
                        break;

                    case "m_LocalPosition.z":
                        tZ = AnimationUtility.GetEditorCurve(clip, bindings[i]);
                        AnimationUtility.SetEditorCurve(clip, bindings[i], null);
                        break;

                    case "m_LocalRotation.x":
                        qX = AnimationUtility.GetEditorCurve(clip, bindings[i]);
                        AnimationUtility.SetEditorCurve(clip, bindings[i], null);
                        break;

                    case "m_LocalRotation.y":
                        qY = AnimationUtility.GetEditorCurve(clip, bindings[i]);
                        AnimationUtility.SetEditorCurve(clip, bindings[i], null);
                        break;

                    case "m_LocalRotation.z":
                        qZ = AnimationUtility.GetEditorCurve(clip, bindings[i]);
                        AnimationUtility.SetEditorCurve(clip, bindings[i], null);
                        break;

                    case "m_LocalRotation.w":
                        qW = AnimationUtility.GetEditorCurve(clip, bindings[i]);
                        AnimationUtility.SetEditorCurve(clip, bindings[i], null);
                        break;
                }
            }

            if (tX != null)
                clip.SetCurve(string.Empty, typeof(Animator), "RootT.x", tX);

            if (tY != null)
                clip.SetCurve(string.Empty, typeof(Animator), "RootT.y", tY);

            if (tZ != null)
                clip.SetCurve(string.Empty, typeof(Animator), "RootT.z", tZ);

            if (qX != null)
                clip.SetCurve(string.Empty, typeof(Animator), "RootQ.x", qX);

            if (qY != null)
                clip.SetCurve(string.Empty, typeof(Animator), "RootQ.y", qY);

            if (qZ != null)
                clip.SetCurve(string.Empty, typeof(Animator), "RootQ.z", qZ);

            if (qW != null)
                clip.SetCurve(string.Empty, typeof(Animator), "RootQ.w", qW);

            EditorUtility.SetDirty(clip);

            AssetDatabase.SaveAssetIfDirty(clip);
        }
    }
}