using UnityEngine;

namespace Akela.Tools
{
    public static class Mathfa
    {
        public static float Sum(params float[] numbers)
        {
            var result = 0f;

            for (var i = 0; i < numbers.Length; i++)
                result += numbers[i];

            return result;
        }

        public static float Average(params float[] numbers)
        {
            return Sum(numbers) / numbers.Length;
        }

        public static Vector3 Sum(params Vector3[] numbers)
        {
            var result = Vector3.zero;

            for (var i = 0; i < numbers.Length; i++)
                result += numbers[i];

            return result;
        }

        public static Vector3 Average(params Vector3[] numbers)
        {
            return Sum(numbers) / numbers.Length;
        }

        public static float StandardDeviation(params float[] numbers)
        {
            var mean = Average(numbers);

            for (var i = 0; i < numbers.Length; ++i)
            {
                var deltaMean = numbers[i] - mean;
                numbers[i] = deltaMean * deltaMean;
            }

            var squaredDeltaMean = Average(numbers);

            return Mathf.Sqrt(squaredDeltaMean);
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            angle = Mathf.Repeat(angle, 360);
            min = Mathf.Repeat(min, 360);
            max = Mathf.Repeat(max, 360);

            var inverse = false;

            var tmin = min;
            var tangle = angle;

            if (min > 180)
            {
                inverse = true;
                tmin -= 180;
            }

            if (angle > 180)
            {
                inverse = !inverse;
                tangle -= 180;
            }

            var result = !inverse ? tangle > tmin : tangle < tmin;

            if (!result)
                angle = min;

            inverse = false;
            tangle = angle;

            var tmax = max;

            if (angle > 180)
            {
                inverse = true;
                tangle -= 180;
            }

            if (max > 180)
            {
                inverse = !inverse;
                tmax -= 180;
            }

            result = !inverse ? tangle < tmax : tangle > tmax;

            if (!result)
                angle = max;

            return angle;
        }
    }
}