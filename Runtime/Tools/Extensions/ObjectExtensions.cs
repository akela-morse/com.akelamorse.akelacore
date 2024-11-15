using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Akela.Tools
{
    public static class ObjectExtensions
    {
        public static void InvokeWithString(this Object obj, string methodName, string parameters)
        {
            var paramArray =
                Regex.Split(parameters, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) *, *(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)")
                .Select(x =>
                {
                    if (x == "false" || x == "true")
                        return (object)x == "true";
                    else if (int.TryParse(x, out var resInt))
                        return (object)resInt;
                    else if (float.TryParse(x, out var resFloat))
                        return resFloat;
                    else
                        return x;
                })
                .ToArray();

            obj.GetType().GetMethod(methodName).Invoke(obj, paramArray);
        }

        public static void PlaymodeAgnosticDestroy(this Object obj)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                Object.Destroy(obj);
            else
                Object.DestroyImmediate(obj);
#else
            Object.Destroy(obj);
#endif
        }
    }
}