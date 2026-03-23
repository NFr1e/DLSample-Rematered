#if UNITY_EDITOR
using System.IO;

using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

using DLSample.Shared;
using DLSample.Editor.PathGrapher;

namespace DLSample.Editor.LevelCreator
{
    public static class LevelCreatorHelper
    {
        public static bool CreateLevel(string basePath, string levelName, string soundtrackInfo, int gemCount, AudioClip soundtrackClip)
        {
            //校验
            if (string.IsNullOrEmpty(basePath) || !basePath.StartsWith("Assets")) return false;

            if (string.IsNullOrEmpty(levelName)) return false;

            string levelFolderPath = Path.Combine(basePath, levelName).Replace('\\', '/');
            string fullFolderPath = Path.Combine(Application.dataPath, levelFolderPath["Assets/".Length..]).Replace('\\', '/');

            if (Directory.Exists(fullFolderPath))
            {
                EditorUtility.DisplayDialog("Error", $"已存在: {levelFolderPath}", "OK");
                return false;
            }

            try
            {
                //文件夹
                Directory.CreateDirectory(fullFolderPath);
                AssetDatabase.Refresh();

                //Resource文件夹
                string resourcesFolder = Path.Combine(fullFolderPath, "Resources");
                Directory.CreateDirectory(resourcesFolder);
                AssetDatabase.Refresh();

                //场景
                string scenePath = CreateEmptyScene(levelFolderPath, levelName);
                if (string.IsNullOrEmpty(scenePath))
                {
                    throw new System.Exception();
                }

                //LevelData
                string levelDataPath = Path.Combine(levelFolderPath, $"LevelData_{levelName}.asset").Replace('\\', '/');
                LevelDataScriptable levelData = ScriptableObject.CreateInstance<LevelDataScriptable>();

                levelData.levelScene = GetSceneAssetAtPath(scenePath);
                levelData.sceneName = levelName;
                levelData.levelName = levelName;
                levelData.soundtrackInfo = soundtrackInfo;
                levelData.gemCount = gemCount;
                levelData.levelLength = soundtrackClip != null ? soundtrackClip.length : 0f;

                AssetDatabase.CreateAsset(levelData, levelDataPath);

                //BeatmapData
                string beatmapPath = Path.Combine(levelFolderPath, $"BeatmapData_{levelName}.asset").Replace('\\', '/');
                BeatmapDataScriptable beatmapData = ScriptableObject.CreateInstance<BeatmapDataScriptable>();
                AssetDatabase.CreateAsset(beatmapData, beatmapPath);

                //PathGrapherAsset
                string pathGrapherPath = Path.Combine(levelFolderPath, $"PathGrapherAsset_{levelName}.asset").Replace('\\', '/');
                PathGrapherAsset pathGrapher = ScriptableObject.CreateInstance<PathGrapherAsset>();
                AssetDatabase.CreateAsset(pathGrapher, pathGrapherPath);

                BeatmapDataScriptable loadedBeatmap = AssetDatabase.LoadAssetAtPath<BeatmapDataScriptable>(beatmapPath);
                PathGrapherAsset loadedPathGrapher = AssetDatabase.LoadAssetAtPath<PathGrapherAsset>(pathGrapherPath);
                loadedPathGrapher.beatMapData = loadedBeatmap;

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                EditorUtility.DisplayDialog("Success", $"Level [\"{levelName}\"] created at:\n {levelFolderPath}", "OK");
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error: {ex.Message}\n{ex.StackTrace}");
                EditorUtility.DisplayDialog("Error", $"Creating Failed: {ex.Message}", "OK");
                return false;
            }
        }

        private static string CreateEmptyScene(string folderRelativePath, string sceneName)
        {
            bool originalSceneDirty = SceneManager.GetActiveScene().isDirty;

            if (originalSceneDirty)
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            }

            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            string scenePath = Path.Combine(folderRelativePath, $"{sceneName}.unity").Replace('\\', '/');

            EditorSceneManager.SaveScene(newScene, scenePath);

            return scenePath;
        }

        private static SceneAsset GetSceneAssetAtPath(string path)
        {
            return AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
        }
    }
}
#endif