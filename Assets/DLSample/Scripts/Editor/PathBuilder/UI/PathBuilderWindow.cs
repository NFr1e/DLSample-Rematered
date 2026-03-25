using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using DLSample.Shared;

namespace DLSample.Editor.PathBuilder
{
    public class PathBuilderWindow : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        private PathBuilderController _controller;

        [MenuItem(
            itemName: DLSampleConsts.Editor.MENU_ITEM_PATH_BUILDER,
            priority = DLSampleConsts.Editor.MENU_ITEM_PATH_BUILDER_PRIORITY)]
        public static void OpenWindow()
        {
            PathBuilderWindow window = GetWindow<PathBuilderWindow>();

            window.titleContent = new GUIContent("PathBuilderWindow");
            window.minSize = new Vector2(400, 400);
            window.Show();
        }

        public void CreateGUI()
        {
            if (m_VisualTreeAsset == null) return;

            Init();
        }
        private void OnDestroy()
        {
            Dispose();
        }

        private void Init()
        {
            _controller = new(m_VisualTreeAsset, rootVisualElement);
            _controller.Init();
        }
        private void Dispose()
        {
            _controller?.Dispose();
        }
    }
}
