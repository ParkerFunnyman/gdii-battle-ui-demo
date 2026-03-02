using System;
using System.Collections.Generic;
using UnityEngine;

namespace CharlesEngine
{
    [Serializable]
    public class MappedItem
    {
        public KeyCode Code;
        public InputAction Action;
    }

    [CreateAssetMenu(fileName = "inputMapping", menuName = "Input/InputMapping")]
    public class InputMapping : ScriptableObject
    {
        public List<MappedItem> Mappings = new List<MappedItem>();

        [NonSerialized] private Dictionary<InputAction, KeyCode> _dictionary;

        private void OnEnable() => Rebuild();

#if UNITY_EDITOR
        private void OnValidate() => Rebuild();
#endif

        private void Rebuild()
        {
            if (_dictionary == null) _dictionary = new Dictionary<InputAction, KeyCode>();
            else _dictionary.Clear();

            foreach (var item in Mappings)
            {
                // Last one wins if duplicates exist
                _dictionary[item.Action] = item.Code;
            }
        }

        public bool TryGetCode(InputAction action, out KeyCode code)
        {
            if (_dictionary == null || _dictionary.Count == 0) Rebuild();
            return _dictionary.TryGetValue(action, out code);
        }

        public KeyCode GetCode(InputAction action)
        {
            if (TryGetCode(action, out var code)) return code;

            Debug.LogError($"InputMapping '{name}' has no mapping for action '{action}'.");
            return KeyCode.None;
        }

        public KeyCode this[InputAction action] => GetCode(action);
    }
}