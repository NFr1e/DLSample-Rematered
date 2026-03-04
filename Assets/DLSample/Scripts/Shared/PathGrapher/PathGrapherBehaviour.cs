#if UNITY_EDITOR
using UnityEngine;

namespace DLSample.Editor.PathGrapher
{
    [ExecuteInEditMode]
    public class PathGrapherBehaviour : MonoBehaviour
    {
        public PathGrapherAsset asset;
        public PathGrapherProfile profile;

        private void OnValidate()
        {
            RequestRebuild();
        }

        public void RequestRebuild()
        {
            if (asset != null)
            {
                PathSimulator.Simulate(asset, profile.samplingInterval);
            }
        }
    }
}
#endif