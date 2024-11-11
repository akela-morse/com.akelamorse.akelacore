using System.Collections;
using UnityEngine;

namespace Akela.Tools
{
	[AddComponentMenu("Tools/Invokable")]
	public class Invokable : MonoBehaviour
	{
		public void DestroyMe(float time) => Destroy(gameObject, time);

		public void DestroyMe() => Destroy(gameObject);

		public void DestroyMeNextFrame() => StartCoroutine(DestroyNextFrame());

		public void DestroyGameObject(GameObject target) => Destroy(target);

		public void DestroyComponent(Component target) => Destroy(target);

		public void DisableMe(float time) => StartCoroutine(DisableAfterTime(time));

		public void DisableMeNextFrame() => StartCoroutine(DisableNextFrame());

		public void Parent(Transform parent) => transform.SetParent(parent);

		public void Parent(Transform target, Transform parent) => target.SetParent(parent);

		public void Unparent() => transform.SetParent(null);

		public void Unparent(Transform target) => target.SetParent(null);

#if UNITY_EDITOR
		public void DebugLog(string value) => Debug.Log(value, this);
#endif

		#region Private Methods
		private IEnumerator DestroyNextFrame()
		{
			yield return null;
			Destroy(gameObject);
		}

		private IEnumerator DisableAfterTime(float time)
		{
			yield return new WaitForSeconds(time);
			gameObject.SetActive(false);
		}

		private IEnumerator DisableNextFrame()
		{
			yield return null;
			gameObject.SetActive(false);
		}
		#endregion
	}
}
