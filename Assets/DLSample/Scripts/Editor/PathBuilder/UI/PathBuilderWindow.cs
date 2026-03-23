using DLSample.Shared;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PathBuilderWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem(
        itemName:DLSampleConsts.Editor.MENU_ITEM_PATH_BUILDER,
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
        VisualElement root = rootVisualElement;
        m_VisualTreeAsset.CloneTree(root);
    }
}
