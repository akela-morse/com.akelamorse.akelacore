using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Internal;

namespace Akela.Tools
{
    // ReSharper disable InvalidXmlDocComment
    public interface IComponent
    {
        /// <summary>
        ///   <para>The Transform attached to this GameObject.</para>
        /// </summary>
        Transform transform { get; }

        /// <summary>
        ///   <para>The TransformHandle of this GameObject.</para>
        /// </summary>
        TransformHandle transformHandle { get; }

        /// <summary>
        ///   <para>The game object this component is attached to. A component is always attached to a game object.</para>
        /// </summary>
        GameObject gameObject { get; }

        /// <summary>
        ///   <para>The non-generic version of this method.</para>
        /// </summary>
        /// <param name="type">The type of Component to retrieve.</param>
        /// <returns>
        ///   <para>A Component of the matching type, otherwise null if no Component is found.</para>
        /// </returns>
        Component GetComponent(Type type);

        T GetComponent<T>();

        bool TryGetComponent(Type type, out Component component);

        bool TryGetComponent<T>(out T component);

        /// <summary>
        ///   <para>The string-based version of this method.</para>
        /// </summary>
        /// <param name="type">The name of the type of Component to get.</param>
        /// <returns>
        ///   <para>A Component of the matching type, otherwise null if no Component is found.</para>
        /// </returns>
        Component GetComponent(string type);

        /// <summary>
        ///   <para>This is the non-generic version of this method.</para>
        /// </summary>
        /// <param name="t">The type of component to search for.</param>
        /// <param name="includeInactive">Whether to include inactive child GameObjects in the search.</param>
        /// <returns>
        ///   <para>A Component of the matching type, otherwise null if no Component is found.</para>
        /// </returns>
        Component GetComponentInChildren(Type t, bool includeInactive);

        /// <summary>
        ///   <para>This is the non-generic version of this method.</para>
        /// </summary>
        /// <param name="t">The type of component to search for.</param>
        /// <param name="includeInactive">Whether to include inactive child GameObjects in the search.</param>
        /// <returns>
        ///   <para>A Component of the matching type, otherwise null if no Component is found.</para>
        /// </returns>
        Component GetComponentInChildren(Type t);

        T GetComponentInChildren<T>([DefaultValue("false")] bool includeInactive);

        T GetComponentInChildren<T>();

        /// <summary>
        ///   <para>The non-generic version of this method.</para>
        /// </summary>
        /// <param name="t">The type of component to search for.</param>
        /// <param name="includeInactive">Whether to include inactive child GameObjects in the search.</param>
        /// <returns>
        ///   <para>An array of all found components matching the specified type.</para>
        /// </returns>
        Component[] GetComponentsInChildren(Type t, bool includeInactive);

        Component[] GetComponentsInChildren(Type t);

        T[] GetComponentsInChildren<T>(bool includeInactive);

        void GetComponentsInChildren<T>(bool includeInactive, List<T> result);

        T[] GetComponentsInChildren<T>();

        void GetComponentsInChildren<T>(List<T> results);

        /// <summary>
        ///   <para>The non-generic version of this method.</para>
        /// </summary>
        /// <param name="t">The type of component to search for.</param>
        /// <param name="includeInactive">Whether to include inactive GameObjects in the search.</param>
        /// <returns>
        ///   <para>A Component of the matching type, otherwise null if no Component is found.</para>
        /// </returns>
        Component GetComponentInParent(Type t, bool includeInactive);

        /// <summary>
        ///   <para>The non-generic version of this method.</para>
        /// </summary>
        /// <param name="t">The type of component to search for.</param>
        /// <param name="includeInactive">Whether to include inactive GameObjects in the search.</param>
        /// <returns>
        ///   <para>A Component of the matching type, otherwise null if no Component is found.</para>
        /// </returns>
        Component GetComponentInParent(Type t);

        T GetComponentInParent<T>([DefaultValue("false")] bool includeInactive);

        T GetComponentInParent<T>();

        /// <summary>
        ///   <para>The non-generic version of this method.</para>
        /// </summary>
        /// <param name="t">The type of component to search for.</param>
        /// <param name="includeInactive">Whether to include inactive GameObjects in the search.</param>
        /// <returns>
        ///   <para>An array of all found components matching the specified type.</para>
        /// </returns>
        Component[] GetComponentsInParent(Type t, [DefaultValue("false")] bool includeInactive);

        Component[] GetComponentsInParent(Type t);

        T[] GetComponentsInParent<T>(bool includeInactive);

        void GetComponentsInParent<T>(bool includeInactive, List<T> results);

        T[] GetComponentsInParent<T>();

        /// <summary>
        ///   <para>The non-generic version of this method.</para>
        /// </summary>
        /// <param name="type">The type of component to search for.</param>
        /// <returns>
        ///   <para>An array containing all matching components of type type.</para>
        /// </returns>
        Component[] GetComponents(Type type);

        void GetComponents(Type type, List<Component> results);

        void GetComponents<T>(List<T> results);

        /// <summary>
        ///   <para>The tag of this game object.</para>
        /// </summary>
        string tag { get; set; }

        T[] GetComponents<T>();

        /// <summary>
        ///   <para>Gets the index of the component on its parent GameObject.</para>
        /// </summary>
        /// <returns>
        ///   <para>The component index.</para>
        /// </returns>
        int GetComponentIndex();

        /// <summary>
        ///   <para>Checks the GameObject's tag against the defined tag.</para>
        /// </summary>
        /// <param name="tag">The tag to compare.</param>
        /// <returns>
        ///   <para>Returns true if GameObject has same tag. Returns false otherwise.</para>
        /// </returns>
        bool CompareTag(string tag);

        /// <summary>
        ///   <para>Checks the GameObject's tag against the defined tag.</para>
        /// </summary>
        /// <param name="tag">A TagHandle representing the tag to compare.</param>
        /// <returns>
        ///   <para>Returns true if GameObject has same tag. Returns false otherwise.</para>
        /// </returns>
        bool CompareTag(TagHandle tag);

        /// <summary>
        ///   <para>Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.</para>
        /// </summary>
        /// <param name="methodName">Name of method to call.</param>
        /// <param name="value">Optional parameter value for the method.</param>
        /// <param name="options">Should an error be raised if the method does not exist on the target object?</param>
        void SendMessageUpwards(string methodName, [DefaultValue("null")] object value, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

        /// <summary>
        ///   <para>Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.</para>
        /// </summary>
        /// <param name="methodName">Name of method to call.</param>
        /// <param name="value">Optional parameter value for the method.</param>
        /// <param name="options">Should an error be raised if the method does not exist on the target object?</param>
        void SendMessageUpwards(string methodName, object value);

        /// <summary>
        ///   <para>Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.</para>
        /// </summary>
        /// <param name="methodName">Name of method to call.</param>
        /// <param name="value">Optional parameter value for the method.</param>
        /// <param name="options">Should an error be raised if the method does not exist on the target object?</param>
        void SendMessageUpwards(string methodName);

        /// <summary>
        ///   <para>Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.</para>
        /// </summary>
        /// <param name="methodName">Name of method to call.</param>
        /// <param name="value">Optional parameter value for the method.</param>
        /// <param name="options">Should an error be raised if the method does not exist on the target object?</param>
        void SendMessageUpwards(string methodName, SendMessageOptions options);

        /// <summary>
        ///   <para>Calls the method named methodName on every MonoBehaviour in this game object.</para>
        /// </summary>
        /// <param name="methodName">Name of the method to call.</param>
        /// <param name="value">Optional parameter for the method.</param>
        /// <param name="options">Should an error be raised if the target object doesn't implement the method for the message?</param>
        void SendMessage(string methodName, object value);

        /// <summary>
        ///   <para>Calls the method named methodName on every MonoBehaviour in this game object.</para>
        /// </summary>
        /// <param name="methodName">Name of the method to call.</param>
        /// <param name="value">Optional parameter for the method.</param>
        /// <param name="options">Should an error be raised if the target object doesn't implement the method for the message?</param>
        void SendMessage(string methodName);

        /// <summary>
        ///   <para>Calls the method named methodName on every MonoBehaviour in this game object.</para>
        /// </summary>
        /// <param name="methodName">Name of the method to call.</param>
        /// <param name="value">Optional parameter for the method.</param>
        /// <param name="options">Should an error be raised if the target object doesn't implement the method for the message?</param>
        void SendMessage(string methodName, object value, SendMessageOptions options);

        /// <summary>
        ///   <para>Calls the method named methodName on every MonoBehaviour in this game object.</para>
        /// </summary>
        /// <param name="methodName">Name of the method to call.</param>
        /// <param name="value">Optional parameter for the method.</param>
        /// <param name="options">Should an error be raised if the target object doesn't implement the method for the message?</param>
        void SendMessage(string methodName, SendMessageOptions options);

        /// <summary>
        ///   <para>Calls the method named methodName on every MonoBehaviour in this game object or any of its children.</para>
        /// </summary>
        /// <param name="methodName">Name of the method to call.</param>
        /// <param name="parameter">Optional parameter to pass to the method (can be any value).</param>
        /// <param name="options">Should an error be raised if the method does not exist for a given target object?</param>
        void BroadcastMessage(string methodName, [DefaultValue("null")] object parameter, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

        /// <summary>
        ///   <para>Calls the method named methodName on every MonoBehaviour in this game object or any of its children.</para>
        /// </summary>
        /// <param name="methodName">Name of the method to call.</param>
        /// <param name="parameter">Optional parameter to pass to the method (can be any value).</param>
        /// <param name="options">Should an error be raised if the method does not exist for a given target object?</param>
        void BroadcastMessage(string methodName, object parameter);

        /// <summary>
        ///   <para>Calls the method named methodName on every MonoBehaviour in this game object or any of its children.</para>
        /// </summary>
        /// <param name="methodName">Name of the method to call.</param>
        /// <param name="parameter">Optional parameter to pass to the method (can be any value).</param>
        /// <param name="options">Should an error be raised if the method does not exist for a given target object?</param>
        void BroadcastMessage(string methodName);

        /// <summary>
        ///   <para>Calls the method named methodName on every MonoBehaviour in this game object or any of its children.</para>
        /// </summary>
        /// <param name="methodName">Name of the method to call.</param>
        /// <param name="parameter">Optional parameter to pass to the method (can be any value).</param>
        /// <param name="options">Should an error be raised if the method does not exist for a given target object?</param>
        void BroadcastMessage(string methodName, SendMessageOptions options);

        /// <summary>
        ///   <para>Enabled Behaviours are Updated, disabled Behaviours are not.</para>
        /// </summary>
        bool enabled { get; set; }

        /// <summary>
        ///   <para>Checks whether a component is enabled, attached to a GameObject that is active in the hierarchy, and the component's OnEnable has been called.</para>
        /// </summary>
        bool isActiveAndEnabled { get; }

        /// <summary>
        ///   <para>Cancellation token raised when the MonoBehaviour is destroyed (Read Only).</para>
        /// </summary>
        CancellationToken destroyCancellationToken { get; }

        /// <summary>
        ///   <para>Disabling this lets you skip the GUI layout phase.</para>
        /// </summary>
        bool useGUILayout { get; set; }

        /// <summary>
        ///   <para>Returns a boolean value which represents if Start was called.</para>
        /// </summary>
        bool didStart { get; }

        /// <summary>
        ///   <para>Returns a boolean value which represents if Awake was called.</para>
        /// </summary>
        bool didAwake { get; }

#if UNITY_EDITOR
        /// <summary>
        ///   <para>Allow a specific instance of a MonoBehaviour to run in edit mode (only available in the editor).</para>
        /// </summary>
        bool runInEditMode { get; set; }
#endif

        /// <summary>
        ///   <para>Is any invoke pending on this MonoBehaviour?</para>
        /// </summary>
        bool IsInvoking();

        /// <summary>
        ///   <para>Cancels all Invoke calls on this MonoBehaviour.</para>
        /// </summary>
        void CancelInvoke();

        /// <summary>
        ///   <para>Invokes the method methodName in time seconds.</para>
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="time"></param>
        void Invoke(string methodName, float time);

        /// <summary>
        ///   <para>Invokes the specified method after a specified delay, then repeatedly at the specified rate.</para>
        /// </summary>
        /// <param name="methodName">The name of a method to invoke.</param>
        /// <param name="time">Time to wait in seconds before the first invocation.</param>
        /// <param name="repeatRate">Interval in seconds between method invocations.</param>
        void InvokeRepeating(string methodName, float time, float repeatRate);

        /// <summary>
        ///   <para>Cancels all Invoke calls with name methodName on this behaviour.</para>
        /// </summary>
        /// <param name="methodName"></param>
        void CancelInvoke(string methodName);

        /// <summary>
        ///   <para>Is any invoke on methodName pending?</para>
        /// </summary>
        /// <param name="methodName"></param>
        bool IsInvoking(string methodName);

        /// <summary>
        ///   <para>Starts a coroutine named methodName.</para>
        /// </summary>
        /// <param name="methodName"></param>
        Coroutine StartCoroutine(string methodName);

        /// <summary>
        ///   <para>Starts a coroutine named methodName.</para>
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="value"></param>
        Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value);

        /// <summary>
        ///   <para>Starts a coroutine.</para>
        /// </summary>
        /// <param name="routine"></param>
        Coroutine StartCoroutine(IEnumerator routine);

        /// <summary>
        ///   <para>Stops the first coroutine named methodName, or the coroutine stored in routine running on this behaviour.</para>
        /// </summary>
        /// <param name="methodName">Name of coroutine.</param>
        /// <param name="routine">Name of the function in code, including coroutines.</param>
        void StopCoroutine(IEnumerator routine);

        /// <summary>
        ///   <para>Stops the first coroutine named methodName, or the coroutine stored in routine running on this behaviour.</para>
        /// </summary>
        /// <param name="methodName">Name of coroutine.</param>
        /// <param name="routine">Name of the function in code, including coroutines.</param>
        void StopCoroutine(Coroutine routine);

        /// <summary>
        ///   <para>Stops the first coroutine named methodName, or the coroutine stored in routine running on this behaviour.</para>
        /// </summary>
        /// <param name="methodName">Name of coroutine.</param>
        /// <param name="routine">Name of the function in code, including coroutines.</param>
        void StopCoroutine(string methodName);

        /// <summary>
        ///   <para>Stops all coroutines running on this MonoBehaviour.</para>
        /// </summary>
        void StopAllCoroutines();
    }
}