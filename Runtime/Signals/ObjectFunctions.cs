using Akela.Bridges;
using System.Collections.Generic;
using UnityEngine;

namespace Akela.Signals
{
    [AddComponentMenu("Signals/Object Functions", 4)]
    [DisallowMultipleComponent]
    public class ObjectFunctions : MonoBehaviour, ISerializationCallbackReceiver
    {
        #region Component Fields
        [SerializeField, HideInInspector] List<string> _keys = new();
        [SerializeField, HideInInspector] List<BridgedEvent> _values = new();
        #endregion

        private readonly Dictionary<string, BridgedEvent> _functions = new();

        public void OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();

            foreach (var entries in _functions)
            {
                _keys.Add(entries.Key);
                _values.Add(entries.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            _functions.Clear();

            for (var i = 0; i < _keys.Count; ++i)
            {
#if UNITY_EDITOR
                if (!_functions.TryAdd(_keys[i], _values[i]))
                {
                    Debug.LogError($"Issue while serializing ObjectFunctions. Function name '{_keys[i]}' already exists.", this);
                    continue;
                }
#endif
                _functions.Add(_keys[i], _values[i]);
            }
        }

        public void Call(string name)
        {
            if (!_functions.TryGetValue(name, out var function))
                return;

            function.Invoke();
        }
    }
}
