using Descent.Common.AI.BehaviourTree.Core;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Editor
{
    [CustomPropertyDrawer(typeof(IBehaviourTreeCondition), true)]
    public class BehaviourTreeConditionDrawer : PropertyDrawer
    {
        private static Type[] _types;
        private static string[] _names;

        private void CacheTypes()
        {
            if (_types != null) return;
            _types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract
                         && typeof(IBehaviourTreeCondition).IsAssignableFrom(t)
                         && t.GetCustomAttributes(typeof(SerializableAttribute), false).Any()
                )
                .OrderBy(t => t.Name)
                .ToArray();
            _names = _types.Select(t => t.Name).ToArray();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CacheTypes();
            property.serializedObject.Update();
            EditorGUI.BeginProperty(position, label, property);

            float lh = EditorGUIUtility.singleLineHeight;
            var foldRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, lh);
            var popupRect = new Rect(position.x + EditorGUIUtility.labelWidth,
            position.y,
            position.width - EditorGUIUtility.labelWidth,
            lh);

            property.isExpanded = EditorGUI.Foldout(foldRect, property.isExpanded, label, true);

            var display = new string[_names.Length + 1];
            display[0] = "<None>";
            Array.Copy(_names, 0, display, 1, _names.Length);

            var curType = property.managedReferenceValue?.GetType();
            int curIndex = curType == null
                ? 0
                : Array.FindIndex(_types, t => t == curType) + 1;

            EditorGUI.BeginChangeCheck();
            int newIndex = EditorGUI.Popup(popupRect, curIndex, display);
            if (EditorGUI.EndChangeCheck())
            {
                if (newIndex == 0)
                    property.managedReferenceValue = null;
                else
                    property.managedReferenceValue = Activator.CreateInstance(_types[newIndex - 1]);
            }

            if (property.isExpanded && property.managedReferenceValue != null)
            {
                var childPos = new Rect(position.x,
                                        position.y + lh + EditorGUIUtility.standardVerticalSpacing,
                                        position.width,
                                        GetPropertyHeight(property, label) - lh);
                EditorGUI.PropertyField(childPos, property, true);
            }

            EditorGUI.EndProperty();
            property.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float h = EditorGUIUtility.singleLineHeight;
            if (property.isExpanded && property.managedReferenceValue != null)
            {
                h += EditorGUI.GetPropertyHeight(property, true)
                   + EditorGUIUtility.standardVerticalSpacing;
            }
            return h;
        }
    }
}