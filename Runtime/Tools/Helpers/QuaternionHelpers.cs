using UnityEngine;

namespace Akela.Tools
{
	public static class QuaternionHelpers
	{
		public static Quaternion OmniLookRotation(Vector3 aimAxis, Vector3 direction, Vector3 upAxis)
		{
			return OmniLookRotation(aimAxis, direction, upAxis, Vector3.up);
		}

		public static Quaternion OmniLookRotation(Vector3 aimAxis, Vector3 direction, Vector3 upAxis, Vector3 worldUp)
		{
			var zyToCustom = Quaternion.LookRotation(aimAxis, upAxis);
			var customToZY = Quaternion.Inverse(zyToCustom);

			var zyToTarget = Quaternion.LookRotation(direction, worldUp);

			var customToTarget = zyToTarget * customToZY;

			return customToTarget;
		}
	}
}
