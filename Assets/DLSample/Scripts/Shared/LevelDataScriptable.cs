using UnityEditor;
using UnityEngine;

namespace DLSample.Shared
{
    [CreateAssetMenu(
        menuName = DLSampleConsts.Editor.CREATE_MENU_LEVELDATA_MENU_NAME, 
        fileName = DLSampleConsts.Editor.CREATE_MENU_LEVELDATA_FILE_NAME, 
        order = DLSampleConsts.Editor.CREATE_MENU_LEVELDATA_ORDER)]
    public class LevelDataScriptable : ScriptableObject
    {
        [SerializeField] private SceneAsset levelScene;
        [SerializeField] private string levelName = "LevelName";
        [SerializeField] private string soundtrackInfo = "SoundtrackInfo";
        [SerializeField] private int gemCount = 10;
        [SerializeField] private float levelLength = 100;

        public SceneAsset LevelScene => levelScene;
        public string LevelName => levelName;
        public string SoundtrackInfo => soundtrackInfo;
        public int GemCount => gemCount;
        public float LevelLength => levelLength;
    }
}
