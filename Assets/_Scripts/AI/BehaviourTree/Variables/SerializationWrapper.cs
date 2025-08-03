using System;
using UnityEngine;

namespace Descent.AI.BehaviourTree.Variables
{
    [Serializable]
    public class SerializationWrapper
    {
        [SerializeField] private int _intValue;
        [SerializeField] private float _floatValue;
        [SerializeField] private bool _boolValue;
        [SerializeField] private string _stringValue;
        [SerializeField] private Vector3 _vector3Value;
        [SerializeField] private Vector2 _vector2Value;
        [SerializeField] private Color _colorValue;
        [SerializeField] private VariableType _type;

        public SerializationWrapper() { }

        public SerializationWrapper(object obj)
        {
            SetValue(obj);
        }

        public object GetValue()
        {
            return _type switch
            {
                VariableType.Int => _intValue,
                VariableType.Float => _floatValue,
                VariableType.Bool => _boolValue,
                VariableType.String => _stringValue,
                VariableType.Vector2 => _vector2Value,
                VariableType.Vector3 => _vector3Value,
                VariableType.Color => _colorValue,
                _ => null
            };
        }

        public void SetValue(object obj)
        {
            if (obj == null)
            {
                return;
            }

            switch (obj)
            {
                case int i:
                    _intValue = i;
                    _type = VariableType.Int;
                    break;

                case float f:
                    _floatValue = f;
                    _type = VariableType.Float;
                    break;

                case bool b:
                    _boolValue = b;
                    _type = VariableType.Bool;
                    break;

                case string s:
                    _stringValue = s;
                    _type = VariableType.String;
                    break;

                case Vector2 v2:
                    _vector2Value = v2;
                    _type = VariableType.Vector2;
                    break;

                case Vector3 v3:
                    _vector3Value = v3;
                    _type = VariableType.Vector3;
                    break;

                case Color c:
                    _colorValue = c;
                    _type = VariableType.Color;
                    break;
            }
        }
    }
}