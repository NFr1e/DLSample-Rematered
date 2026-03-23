#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DLSample.Editor.LevelCreator
{
    public class LevelCreatorController
    {
        private readonly VisualTreeAsset _visualTree;
        private readonly LevelCreatorWindow _window;

        private string _selectedBasePath = "";

        #region UIElements
        private Label _pathDisplayLabel;
        private Button _pathSelectBtn;

        private TextField _levelNameField;
        private TextField _soundtrackInfoField;
        private ObjectField _soundtrackField;
        private IntegerField _gemCountField;

        private Button _confirmBtn;
        private Button _cancelBtn;

        #region Mapper
        private readonly string _class_pathDisplayLabel = "path-display-label";
        private readonly string _class_pathSelectBtn = "path-select-btn";

        private readonly string _class_levelNameField = "config-levelname-field";
        private readonly string _class_soundtrackInfoField = "config-soundtrackinfo-field";
        private readonly string _class_sountrackField = "config-soundtrack-field";
        private readonly string _class_gemCountField = "config-gemcount-field";

        private readonly string _class_confirmBtn = "btn-confirm";
        private readonly string _class_cancelBtn = "btn-cancel";
        #endregion
        #endregion

        public LevelCreatorController(VisualTreeAsset visualTree, LevelCreatorWindow window)
        {
            _visualTree = visualTree;
            _window = window;
        }

        public void Init(VisualElement root)
        {
            if (_visualTree == null) return;

            _visualTree.CloneTree(root);

            GetElements(root);
            InitElements();
            SubscribeEvents();

            SetDefaultPath();
        }

        public void Dispose()
        {
            UnsubscribeEvents();
        }

        private void GetElements(VisualElement root)
        {
            _pathDisplayLabel = root.Q<Label>(className: _class_pathDisplayLabel);
            _pathSelectBtn = root.Q<Button>(className: _class_pathSelectBtn);

            _levelNameField = root.Q<TextField>(className: _class_levelNameField);
            _soundtrackInfoField = root.Q<TextField>(className: _class_soundtrackInfoField);
            _soundtrackField = root.Q<ObjectField>(className: _class_sountrackField);
            _gemCountField = root.Q<IntegerField>(className: _class_gemCountField);

            _confirmBtn = root.Q<Button>(className: _class_confirmBtn);
            _cancelBtn = root.Q<Button>(className: _class_cancelBtn);
        }

        private void InitElements()
        {
            _soundtrackField.objectType = typeof(AudioClip);
            _gemCountField.value = 10;
        }

        private void SubscribeEvents()
        {
            _pathSelectBtn.RegisterCallback<ClickEvent>(OnPathSelectClicked);
            _confirmBtn.RegisterCallback<ClickEvent>(OnConfirmClicked);
            _cancelBtn.RegisterCallback<ClickEvent>(OnCancelClicked);
        }

        private void UnsubscribeEvents()
        {
            _pathSelectBtn.UnregisterCallback<ClickEvent>(OnPathSelectClicked);
            _confirmBtn.UnregisterCallback<ClickEvent>(OnConfirmClicked);
            _cancelBtn.UnregisterCallback<ClickEvent>(OnCancelClicked);
        }

        private void SetDefaultPath()
        {
            string defaultPath = "Assets/DLSample/Levels";
            
            string fullPath = Path.Combine(Application.dataPath, "DLSample/Levels").Replace('\\', '/');
            if (Directory.Exists(fullPath))
            {
                _selectedBasePath = defaultPath;
                _pathDisplayLabel.text = _selectedBasePath;
            }
            else
            {
                _selectedBasePath = "Assets";
                _pathDisplayLabel.text = _selectedBasePath;
            }
        }

        private void OnPathSelectClicked(ClickEvent _)
        {
            string initialPath = Application.dataPath;
            if (!string.IsNullOrEmpty(_selectedBasePath) && _selectedBasePath.StartsWith("Assets"))
            {
                string relative = _selectedBasePath.Substring("Assets".Length);
                string testPath = Application.dataPath + relative;
                if (Directory.Exists(testPath))
                {
                    initialPath = testPath;
                }
            }

            string selectedFolder = EditorUtility.OpenFolderPanel("选择关卡保存位置", initialPath, "");
            if (string.IsNullOrEmpty(selectedFolder))
                return;

            string dataPath = Application.dataPath;
            if (!selectedFolder.StartsWith(dataPath))
            {
                EditorUtility.DisplayDialog("错误", "请选择 Assets 目录下的文件夹", "确定");
                return;
            }

            string relativePath = "Assets" + selectedFolder.Substring(dataPath.Length).Replace('\\', '/');
            _selectedBasePath = relativePath;
            _pathDisplayLabel.text = _selectedBasePath;
        }

        private void OnConfirmClicked(ClickEvent _)
        {
            string levelName = _levelNameField.value?.Trim();
            string soundtrackInfo = _soundtrackInfoField.value;
            int gemCount = _gemCountField.value;
            AudioClip soundtrackClip = _soundtrackField.value as AudioClip;

            if (string.IsNullOrEmpty(_selectedBasePath))
            {
                EditorUtility.DisplayDialog("错误", "请先选择保存路径", "确定");
                return;
            }

            if (string.IsNullOrEmpty(levelName))
            {
                EditorUtility.DisplayDialog("错误", "请输入关卡名称", "确定");
                return;
            }

            char[] invalidChars = Path.GetInvalidFileNameChars();
            if (levelName.IndexOfAny(invalidChars) >= 0)
            {
                EditorUtility.DisplayDialog("错误", "关卡名称包含非法字符", "确定");
                return;
            }

            bool success = LevelCreatorHelper.CreateLevel(_selectedBasePath, levelName, soundtrackInfo, gemCount, soundtrackClip);
            if (success)
                _window.Close();
        }

        private void OnCancelClicked(ClickEvent _)
        {
            _window.Close();
        }
    }
}
#endif