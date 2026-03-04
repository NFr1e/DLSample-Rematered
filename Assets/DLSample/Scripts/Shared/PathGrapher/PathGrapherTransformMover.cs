using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DLSample.Editor.PathGrapher
{
    public class PathGrapherTransformMover : MonoBehaviour
    {
        [SerializeField] private PathGrapherBehaviour behaviour;
        [SerializeField] private bool autoSetTransform = false;

        private double time = 0;

        [OnInspectorGUI]
        public void DrawInspector()
        {
            EditorGUI.BeginChangeCheck();
            time = EditorGUILayout.DoubleField("Time", time);
            if (EditorGUI.EndChangeCheck())
            {
                if (autoSetTransform)
                    transform.position = GetPosition();
            }

            if (autoSetTransform) return;

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
}
