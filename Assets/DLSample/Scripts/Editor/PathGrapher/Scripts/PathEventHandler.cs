using UnityEditor;
using UnityEngine;

namespace DLSample.Editor.PathGrapher
{
    public static class PathEventHandler
    {
        private static readonly Color eventSelectHandleColor = new(1f, 1f, 1f, 0.5f);

        private static IPathEvent _draggingEvent = null;

        #region Default
        /// <summary>
        /// 基础事件操作
        /// </summary>
        public static void Handle(this IPathEvent evt, ref IPathEvent selected, PathGrapherBehaviour behaviour, PathGrapherBehaviourEditor editor)
        {
            Vector3 pos = PathMappingUtility.GetWorldPosFromTime(evt.GlobalTime, behaviour.asset.pathData, behaviour.transform, behaviour.profile.samplingInterval);
            float size = 0.5f;

            Handles.color = eventSelectHandleColor;

            if (Handles.Button(pos, Quaternion.identity, size, size, Handles.SphereHandleCap))
            {
                selected = evt;
                RefreshEditorSelectEvent(selected, editor);
            }

            if (selected == evt)
            {
                EditorGUI.BeginChangeCheck();

                Vector3 newPos = Handles.PositionHandle(pos, Quaternion.identity);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(behaviour.asset, "Edit Event");

                    evt.GlobalTime = PathMappingUtility.FindNearestTimeOnPath(newPos, behaviour.asset.pathData, behaviour.transform, behaviour.profile.samplingInterval);
                    behaviour.RequestRebuild();

                    editor.Repaint();
                }
            }
        }
        #endregion

        #region ForceTurn

        private static bool _isDraggingTurn = false;
        private static Vector3 _tempTurnWorldPos;

        /// <summary>
        /// 强制转向事件操作
        /// </summary>
        public static void Handle(this ForceTurnEvent evt, ref IPathEvent selected, PathGrapherBehaviour behaviour, PathGrapherBehaviourEditor editor)
        {
            Vector3 pos = PathMappingUtility.GetWorldPosFromTime(evt.GlobalTime, behaviour.asset.pathData, behaviour.transform, behaviour.profile.samplingInterval);
            float size = 0.5f;

            Handles.color = eventSelectHandleColor;

            if (Handles.Button(pos, Quaternion.identity, size, size, Handles.SphereHandleCap))
            {
                selected = evt;
                RefreshEditorSelectEvent(selected, editor);
            }

            if(selected == evt)
            {
                int handleID = GUIUtility.GetControlID("TurnHandle".GetHashCode(), FocusType.Passive);

                Vector3 realWorldPos = PathMappingUtility.GetWorldPosFromTime(evt.GlobalTime, behaviour.asset.pathData, behaviour.transform, behaviour.profile.samplingInterval);
                Vector3 displayPos = (_isDraggingTurn && _draggingEvent == evt) ? _tempTurnWorldPos : realWorldPos;

                EditorGUI.BeginChangeCheck();
                Vector3 nextPos = Handles.DoPositionHandle(displayPos, Quaternion.identity);

                if (EditorGUI.EndChangeCheck())
                {
                    _isDraggingTurn = true;
                    _draggingEvent = evt;
                    _tempTurnWorldPos = nextPos;
                }

                if (_isDraggingTurn && _draggingEvent == evt && GUIUtility.hotControl == 0)
                {
                    _isDraggingTurn = false;
                    _draggingEvent = null;

                    Undo.RecordObject(behaviour.asset, "Move Turn");

                    double newTime = PathMappingUtility.FindNearestTimeOnPath(_tempTurnWorldPos, behaviour.asset.pathData, behaviour.transform, behaviour.profile.samplingInterval);

                    PathSegment segment = PathMappingUtility.GetSegmentAtTime(evt.GlobalTime, behaviour.asset.pathData);

                    double endWpTime = segment.endWaypoint.time;
                    double prevWpTime = segment.startWaypoint.time;
                    double newWpTime = PathMappingUtility.GetSegmentAtTime(evt.GlobalTime + 0.01f, behaviour.asset.pathData).endWaypoint.time;
                    evt.GlobalTime = System.Math.Clamp(newTime, prevWpTime, newWpTime);

                    behaviour.RequestRebuild();
                    editor.Repaint();
                }
            }
        }
        #endregion

        #region JumpEvent

        private static bool _isDraggingJumpEnd = false;
        private static Vector3 _tempJumpEndWorldPos;

        /// <summary>
        /// 跳跃事件操作
        /// </summary>
        public static void Handle(this JumpEvent evt, ref IPathEvent selected, PathGrapherBehaviour behaviour, PathGrapherBehaviourEditor editor)
        {
            Vector3 pos = PathMappingUtility.GetWorldPosFromTime(evt.GlobalTime, behaviour.asset.pathData, behaviour.transform, behaviour.profile.samplingInterval);
            float size = 0.5f;

            Handles.color = eventSelectHandleColor;

            if (Handles.Button(pos, Quaternion.identity, size, size, Handles.SphereHandleCap))
            {
                selected = evt;
                RefreshEditorSelectEvent(selected, editor);
            }

            if (selected == evt)
            {
                #region StartPoint
                EditorGUI.BeginChangeCheck();

                Vector3 newStartPos = Handles.PositionHandle(pos, Quaternion.identity);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(behaviour.asset, "Move Jump Start");

                    evt.GlobalTime = PathMappingUtility.FindNearestTimeOnPath(newStartPos, behaviour.asset.pathData, behaviour.transform, behaviour.profile.samplingInterval);
                    behaviour.RequestRebuild();

                    editor.Repaint();
                }
                #endregion

                #region EndPoint

                // 为了减少坐标跳动，此处交互采用拖拽后吸附的方法

                int handleID = GUIUtility.GetControlID("JumpEndHandle".GetHashCode(), FocusType.Passive);

                Vector3 realWorldPos = PathMappingUtility.GetWorldPosFromTime(evt.EndTime, behaviour.asset.pathData, behaviour.transform, behaviour.profile.samplingInterval);
                Vector3 displayPos = (_isDraggingJumpEnd && _draggingEvent == evt) ? _tempJumpEndWorldPos : realWorldPos;

                EditorGUI.BeginChangeCheck();
                Vector3 nextPos = Handles.DoPositionHandle(displayPos, Quaternion.identity);

                if (EditorGUI.EndChangeCheck())
                {
                    _isDraggingJumpEnd = true;
                    _draggingEvent = evt;
                    _tempJumpEndWorldPos = nextPos;
                }

                if (_isDraggingJumpEnd && _draggingEvent == evt && GUIUtility.hotControl == 0)
                {
                    _isDraggingJumpEnd = false;
                    _draggingEvent = null;

                    Undo.RecordObject(behaviour.asset, "Move Jump End");

                    double newTime = PathMappingUtility.FindNearestTimeOnPath(_tempJumpEndWorldPos, behaviour.asset.pathData, behaviour.transform, behaviour.profile.samplingInterval);

                    PathSegment segment = PathMappingUtility.GetSegmentAtTime(evt.GlobalTime, behaviour.asset.pathData);

                    double nextWpTime = segment.endWaypoint.time;
                    evt.EndTime = System.Math.Clamp(newTime, evt.StartTime + 0.01, nextWpTime);

                    behaviour.RequestRebuild();
                }

                #endregion
            }
        }
        #endregion

        #region TeleportEvent
        /// <summary>
        /// 传送事件操作
        /// </summary>
        public static void Handle(this TeleportEvent evt, ref IPathEvent selected, PathGrapherBehaviour behaviour, PathGrapherBehaviourEditor editor)
        {
            Vector3 pos = PathMappingUtility.GetWorldPosFromTime(evt.GlobalTime, behaviour.asset.pathData, behaviour.transform, behaviour.profile.samplingInterval);
            float size = 0.5f;

            Handles.color = eventSelectHandleColor;

            if (Handles.Button(pos, Quaternion.identity, size, size, Handles.SphereHandleCap))
            {
                selected = evt;
                RefreshEditorSelectEvent(selected, editor);
            }

            if (selected == evt)
            {
                #region StartPoint
                // 关于TeleportEvent的StartPoint操作：
                // 由于其特殊性，即TeleportEvent的起点后无路径（是一段空的PathSection），无法根据PositionHandle返回的世界坐标获取时间信息，故移除。
                #endregion

                #region EndPoint
                EditorGUI.BeginChangeCheck();

                Vector3 newTargetPos = Handles.PositionHandle(evt.targetPosition, Quaternion.identity);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(behaviour.asset, "Move Teleport Target");

                    evt.targetPosition = newTargetPos;
                    behaviour.RequestRebuild();
                }
                #endregion
            }
        }
        
        #endregion

        private static void RefreshEditorSelectEvent(IPathEvent evt, PathGrapherBehaviourEditor editor)
        {
            editor.SelectedEvent = evt;
            editor.Repaint();
        }
    }
}
