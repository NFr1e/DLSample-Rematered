using UnityEngine;
using System.Collections.Generic;
using DLSample.Shared;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace DLSample.Editor.PathGrapher
{
    [CreateAssetMenu(
        menuName = DLSampleConsts.Editor.CREATE_MENU_PATHGRAPHERASSET_MENU_NAME, 
        fileName = DLSampleConsts.Editor.CREATE_MENU_PATHGRAPHERASSET_FILE_NAME,
        order = DLSampleConsts.Editor.CREATE_MENU_PATHGRAPHERASSET_ORDER)]
    public class PathGrapherAsset : ScriptableObject
    {
        [Header("Source Data")]
        public BeatmapDataScriptable beatMapData;

        [Header("Initial State")]
        public Vector3 startPosition = Vector3.zero;
        public float initialSpeed = 12f;
        public Vector3 initialGravity = new(0, -9.81f, 0);
        public PlayerDirections initialDirections;

        public PathData pathData = new();
    }

    [Serializable]
    public class PathData
    {
        [SerializeReference]
        public List<IPathEvent> globalEvents = new();

#if ODIN_INSPECTOR
        [ReadOnly]
#endif
        public List<Waypoint> generatedWaypoints = new();
#if ODIN_INSPECTOR
        [ReadOnly]
#endif
        public List<PathSegment> generatedSegments = new();
    }
}
