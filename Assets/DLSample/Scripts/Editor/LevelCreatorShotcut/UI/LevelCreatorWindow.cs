using DLSample.Shared;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DLSample.Editor.LevelCreator
{
    public class LevelCreatorWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;

        private LevelCreatorController _view;

        [MenuItem(
            itemName: DLSampleConsts.Editor.MENU_ITEM_CREATE_LEVEL,
            priority = DLSampleConsts.Editor.MENU_ITEM_CREATE_LEVEL_PRIORITY)]
        public static void OpenWindow()
        {
            LevelCreatorWindow window = GetWindow<LevelCreatorWindow>();
            window.titleContent = new GUIContent("Level Creator");
            window.minSize = new(600, 300);
            window.Show();
        }

        public void CreateGUI()
        {
            if (m_VisualTreeAsset == null)
            {
                Debug.LogError("[LevelCreator] VisualTreeAsset is not assigned!");
                return;
            }

            Initialize();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Initialize()
        {
            _view = new LevelCreatorController(m_VisualTreeAsset, this);
            _view.Init(rootVisualElement);
        }

        private void Dispose()
        {
            _view?.Dispose();
        }
    }
}
