using UnityEngine;

namespace Akela.Tools
{
    public static class Mathfa
    {
        public static float Sum(params float[] numbers)
        {
            var result = 0.0;

            for (var i = 0; i < numbers.Length; i++)
                result += numbers[i];

            return (float)result;
        }

        public static float Average(params float[] numbers)
        {
            var result = 0.0;

            for (var i = 0; i < numbers.Length; i++)
                result += numbers[i];

            var inverse = 1.0 / numbers.Length;

            result *= inverse;

            return (float)result;
        }

        public static Vector3 Sum(params Vector3[] numbers)
        {
            (double x, double y, double z) result = (0.0, 0.0, 0.0);

            for (var i = 0; i < numbers.Length; i++)
                result = (result.x + numbers[i].x, result.y + numbers[i].y, result.z + numbers[i].z);

            return new((float)result.x, (float)result.y, (float)result.z);
        }

        public static Vector3 Average(params Vector3[] numbers)
        {
            (double x, double y, double z) result = (0.0, 0.0, 0.0);

            for (var i = 0; i < numbers.Length; i++)
                result = (result.x + numbers[i].x, result.y + numbers[i].y, result.z + numbers[i].z);

            var inverse = 1.0 / numbers.Length;

            result = (result.x * inverse, result.y * inverse, result.z * inverse);

            return new((float)result.x, (float)result.y, (float)result.z);
        }

        public static float StandardDeviation(params float[] numbers)
        {
            var mean = 0.0;

            for (var i = 0; i < numbers.Length; i++)
                mean += numbers[i];

            var inverse = 1.0 / numbers.Length;

            mean *= inverse;

            var squaredDeltaMean = 0.0;

            for (var i = 0; i < numbers.Length; ++i)
            {
                var deltaMean = numbers[i] - mean;
                squaredDeltaMean += deltaMean * deltaMean;
            }

            squaredDeltaMean *= inverse;

            return Mathf.Sqrt((float)squaredDeltaMean);
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            angle = Mathf.Repeat(angle, 360f);
            min = Mathf.Repeat(min, 360f);
            max = Mathf.Repeat(max, 360f);

            var inverse = false;

            var tmin = min;
            var tangle = angle;

            if (min > 180f)
            {
                inverse = true;
                tmin -= 180f;
            }

            if (angle > 180f)
            {
                inverse = !inverse;
                tangle -= 180f;
            }

            var result = !inverse ? tangle > tmin : tangle < tmin;

            if (!result)
                angle = min;

            inverse = false;
            tangle = angle;

            var tmax = max;

            if (angle > 180f)
            {
                inverse = true;
                tangle -= 180f;
            }

            if (max > 180f)
            {
                inverse = !inverse;
                tmax -= 180f;
            }

            result = !inverse ? tangle < tmax : tangle > tmax;

            if (!result)
                angle = max;

            return angle;
        }
    }
}