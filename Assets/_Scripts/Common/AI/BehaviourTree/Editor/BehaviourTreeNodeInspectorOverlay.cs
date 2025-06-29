using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Descent.Common.AI.BehaviourTree.Editor
{
    using Editor = UnityEditor.Editor;

    public class BehaviourTreeNodeInspectorOverlay : VisualElement
    {
        private Editor _editor;
        private IMGUIContainer _imgui;

        public BehaviourTreeNodeInspectorOverlay()
        {
            style.position = Position.Absolute;
            style.top = 30;
            style.right = 10;
            style.width = 300;
            style.minHeight = 100;
            style.backgroundColor = new StyleColor(new Color(0f, 0f, 0f, 0.6f));
            style.borderBottomLeftRadius = 6;
            style.borderTopLeftRadius = 6;
            style.paddingLeft = 6;
            style.paddingRight = 6;
            style.paddingTop = 4;
            style.paddingBottom = 4;
            style.unityOverflowClipBox = OverflowClipBox.ContentBox;

            var closeBtn = new Button(() => UpdateSelection(this, null))
            {
                text = "✕"
            };
            closeBtn.style.position = Position.Absolute;
            closeBtn.style.top = 4;
            closeBtn.style.right = 4;
            closeBtn.style.width = 16;
            closeBtn.style.height = 16;
            closeBtn.style.fontSize = 12;
            closeBtn.style.unityTextAlign = TextAnchor.MiddleCenter;
            Add(closeBtn);

            _imgui = new IMGUIContainer(() =>
            {
                if (_editor != null)
                {
                    EditorGUIUtility.labelWidth = 80;
                    _editor.OnInspectorGUI();
                }
            });
            _imgui.style.marginTop = 24;
            _imgui.style.color = new StyleColor(Color.white);
            Add(_imgui);

            visible = false;
        }

        public void UpdateSelection(object sender, Object obj)
        {
            if (_editor != null)
            {
                Object.DestroyImmediate(_editor);
            }

            if (obj == null)
            {
                visible = false;
                return;
            }

            _editor = Editor.CreateEditor(obj);
            visible = true;
            MarkDirtyRepaint();
        }
    }
}