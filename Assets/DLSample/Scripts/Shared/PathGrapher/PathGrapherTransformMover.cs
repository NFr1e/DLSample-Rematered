#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

using UnityEditor;
using UnityEngine;

namespace DLSample.Editor.PathGrapher
{
    public class PathGrapherTransformMover : MonoBehaviour
    {
        [SerializeField] private PathGrapherBehaviour behaviour;
        [SerializeField] private bool autoSetTransformByTime = false;

        private double time = 0;

#if ODIN_INSPECTOR
        [OnInspectorGUI]
        public void DrawInspector()
        {
            EditorGUI.BeginChangeCheck();
            time = EditorGUILayout.DoubleField("Time", time);
            if (EditorGUI.EndChangeCheck())
            {
                if (autoSetTransformByTime)
                    transform.position = GetPosition();
            }

            if (autoSetTransformByTime) return;

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("GetTimeFromTransform"))
            {
                time = PathMappingUtility.FindNearestTimeOnPath(transform.position, behaviour.asset.pathData, behaviour.transform, behaviour.profile.samplingInterval);
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("SetPositionFromTime"))
            {
                transform.position = GetPosition();
            }
            if (GUILayout.Button("SetRotationFromTime"))
            {
                transform.rotation = GetRotation();
            }
            GUILayout.EndHorizontal();
        }

        Vector3 GetPosition()
        {
            return PathMappingUtility.GetWorldPosFromTime(time, behaviour.asset.pathData, behaviour.transform, behaviour.profile.samplingInterval);
        }
        Quaternion GetRotation()
        {
            return PathMappingUtility.GetRotationAtTime(time, behaviour.asset.pathData);
        }
    }
#endif
}
