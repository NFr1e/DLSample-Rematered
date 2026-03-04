#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//!!AI黑箱代码，待整理

namespace DLSample.Editor.PathGrapher
{
    public static class PathMappingUtility
    {
        public static double FindNearestTimeOnPath(Vector3 worldPos, PathData pathData, Transform origin, float samplingInterval = 0.1f)
        {
            if (pathData.generatedSegments.Count == 0) return 0;

            var segments = pathData.generatedSegments;

            Matrix4x4 worldToLocal = origin.worldToLocalMatrix;
            Vector3 localPos = worldToLocal.MultiplyPoint(worldPos);

            double bestTime = 0;
            float minSqrDist = float.MaxValue;

            foreach (var segment in segments)
            {
                if (!segment.IsValid) continue;

                foreach (var section in segment.sections)
                {
                    if (section.points == null || section.points.Length < 2) continue;

                    for (int i = 0; i < section.points.Length - 1; i++)
                    {
                        Vector3 p1 = section.points[i];
                        Vector3 p2 = section.points[i + 1];

                        // 1. 投影点到线段 p1-p2 上
                        Vector3 nearestPointOnLine = ClosestPointOnSegment(p1, p2, localPos);
                        float sqrDist = (localPos - nearestPointOnLine).sqrMagnitude;

                        if (sqrDist < minSqrDist)
                        {
                            minSqrDist = sqrDist;

                            // 2. 计算投影点在线段中的比例 (0~1)
                            float tFactor = GetProjectionFactor(p1, p2, nearestPointOnLine);

                            // 3. 计算该小段线段 [p1, p2] 对应的起始和结束时间
                            double timeAtP1, timeAtP2;

                            if (section.points.Length == 2)
                            {
                                // 直线段：直接映射 Section 的时间范围
                                timeAtP1 = section.startTime;
                                timeAtP2 = section.endTime;
                            }
                            else
                            {
                                // 曲线采样段：根据采样索引计算时间
                                timeAtP1 = section.startTime + (i * samplingInterval);

                                // 最后一个采样点的时间必须严格等于 Section 的 endTime
                                if (i == section.points.Length - 2)
                                    timeAtP2 = section.endTime;
                                else
                                    timeAtP2 = section.startTime + ((i + 1) * samplingInterval);
                            }

                            // 4. 插值得到最终精确时间
                            bestTime = timeAtP1 + (timeAtP2 - timeAtP1) * tFactor;
                        }
                    }
                }
            }
            return bestTime;
        }

        // 辅助：计算点在线段上的投影
        private static Vector3 ClosestPointOnSegment(Vector3 a, Vector3 b, Vector3 p)
        {
            Vector3 ap = p - a;
            Vector3 ab = b - a;
            float magnitudeAB = ab.sqrMagnitude;
            if (magnitudeAB == 0) return a;
            float distance = Vector3.Dot(ap, ab) / magnitudeAB;
            return (distance < 0) ? a : (distance > 1) ? b : a + ab * distance;
        }

        // 辅助：获取投影点在线段上的比例 (0-1)
        // 更稳健的线性投影比例计算
        private static float GetProjectionFactor(Vector3 a, Vector3 b, Vector3 p)
        {
            Vector3 ab = b - a;
            Vector3 ap = p - a;
            float magSq = ab.sqrMagnitude;
            if (magSq == 0) return 0;
            return Mathf.Clamp01(Vector3.Dot(ap, ab) / magSq);
        }

        public static Vector3 GetWorldPosFromTime(double time, PathData pathData, Transform origin, float samplingInterval = 0.1f)
        {
            if(pathData.generatedSegments.Count == 0) return origin.transform.position;

            var segments = pathData.generatedSegments;
            var waypoints = pathData.generatedWaypoints;

            foreach (var segment in segments)
            {
                // 使用微小的误差容忍度处理边界问题
                if (time >= segment.startWaypoint.time && time <= segment.endWaypoint.time)
                {
                    if (!segment.IsValid)
                        return origin.transform.TransformPoint(segment.startWaypoint.position);

                    // 2. 定位 PathSection
                    foreach (var section in segment.sections)
                    {
                        if (time >= section.startTime && time <= section.endTime)
                        {
                            if (section.points == null || section.points.Length < 2) continue;

                            Vector3 localPos;
                            double duration = section.endTime - section.startTime;

                            // 3. 处理插值逻辑
                            if (section.points.Length == 2)
                            {
                                // --- 直线/坍缩片段 ---
                                float t = duration > 0 ? (float)((time - section.startTime) / duration) : 0;
                                localPos = Vector3.Lerp(section.points[0], section.points[1], t);
                            }
                            else
                            {
                                // --- 曲线采样片段 ---
                                double relativeTime = time - section.startTime;

                                // 计算基础索引
                                int index = Mathf.FloorToInt((float)(relativeTime / samplingInterval));
                                index = Mathf.Clamp(index, 0, section.points.Length - 2);

                                // 计算该小步内的插值比例
                                double tAtP1 = index * samplingInterval;
                                double tAtP2 = (index == section.points.Length - 2)
                                               ? duration
                                               : (index + 1) * samplingInterval;

                                double subDuration = tAtP2 - tAtP1;
                                float factor = subDuration > 0 ? (float)((relativeTime - tAtP1) / subDuration) : 0;

                                localPos = Vector3.Lerp(section.points[index], section.points[index + 1], Mathf.Clamp01(factor));
                            }

                            return origin.transform.TransformPoint(localPos);
                        }
                    }
                }
            }

            // 如果时间超出范围，返回起点或终点坐标
            if (waypoints.Count > 0)
            {
                var wp = time < waypoints[0].time ? waypoints[0] : waypoints[^1];
                return origin.transform.TransformPoint(wp.position);
            }

            return origin.transform.position;
        }

        public static PathSegment GetSegmentAtTime(double time, PathData pathData)
        {
            foreach (var seg in pathData.generatedSegments)
            {
                if (time >= seg.startWaypoint.time && time <= seg.endWaypoint.time)
                    return seg;
            }
            return default;
        }

        public static (Vector3 worldPos, double time) FindNearestPointByMouse(Vector2 mousePos, PathData pathData, Transform origin, float samplingInterval = 0.1f)
        {
            if (pathData == null || pathData.generatedSegments.Count == 0)
                return (Vector3.zero, 0);

            Matrix4x4 localToWorld = origin.localToWorldMatrix;

            Vector3 bestWorldPos = Vector3.zero;
            double bestTime = 0;
            float minScreenDist = float.MaxValue;

            // 配置阈值 (保留你的设置)
            const float BROAD_PHASE_THRESHOLD = 80f;
            const float NARROW_PHASE_THRESHOLD = 20f;

            // --- 1. Broad Phase: 快速筛选 Segment ---
            List<PathSegment> candidates = new();

            foreach (var segment in pathData.generatedSegments)
            {
                if (!segment.IsValid) continue;

                bool isCandidate = false;
                // 遍历所有 Section 的所有点进行粗筛
                foreach (var section in segment.sections)
                {
                    for (int i = 0; i < section.points.Length; i++)
                    {
                        Vector3 worldP = localToWorld.MultiplyPoint(section.points[i]);
                        Vector2 screenP = HandleUtility.WorldToGUIPoint(worldP);
                        if (Vector2.Distance(screenP, mousePos) < BROAD_PHASE_THRESHOLD)
                        {
                            isCandidate = true;
                            break;
                        }
                    }
                    if (isCandidate) break;
                }

                if (isCandidate) candidates.Add(segment);
            }

            // --- 2. Narrow Phase: 精算投影 ---
            foreach (var segment in candidates)
            {
                foreach (var section in segment.sections)
                {
                    if (section.points.Length < 2) continue;

                    for (int i = 0; i < section.points.Length - 1; i++)
                    {
                        Vector3 p1 = localToWorld.MultiplyPoint(section.points[i]);
                        Vector3 p2 = localToWorld.MultiplyPoint(section.points[i + 1]);

                        Vector2 s1 = HandleUtility.WorldToGUIPoint(p1);
                        Vector2 s2 = HandleUtility.WorldToGUIPoint(p2);

                        // 计算屏幕空间距离线段的距离
                        float screenDist = HandleUtility.DistancePointLine(mousePos, s1, s2);

                        if (screenDist < minScreenDist && screenDist < NARROW_PHASE_THRESHOLD)
                        {
                            minScreenDist = screenDist;

                            // 计算投影因子 (0-1)
                            float t2D = GetProjectionFactor(s1, s2, mousePos);
                            bestWorldPos = Vector3.Lerp(p1, p2, t2D);

                            // 计算该小段的时间范围
                            double timeAtP1, timeAtP2;
                            if (section.points.Length == 2)
                            {
                                // 直线段
                                timeAtP1 = section.startTime;
                                timeAtP2 = section.endTime;
                            }
                            else
                            {
                                // 曲线采样段
                                timeAtP1 = section.startTime + (i * samplingInterval);
                                timeAtP2 = (i == section.points.Length - 2)
                                           ? section.endTime
                                           : section.startTime + ((i + 1) * samplingInterval);
                            }

                            // 最终插值得到时间
                            bestTime = timeAtP1 + (timeAtP2 - timeAtP1) * (double)t2D;
                        }
                    }
                }
            }

            if (minScreenDist < NARROW_PHASE_THRESHOLD)
            {
                return (bestWorldPos, bestTime);
            }

            return (Vector3.zero, 0);
        }

        public static Quaternion GetRotationAtTime(double time, PathData data)
        {
            for (int i = 0; i < data.generatedWaypoints.Count - 1; i++)
            {
                if (time >= data.generatedWaypoints[i].time && time <= data.generatedWaypoints[i + 1].time)
                {
                    return data.generatedWaypoints[i].rotation;
                }
            }
            return data.generatedWaypoints.Count > 0 ? data.generatedWaypoints[0].rotation : Quaternion.identity;
        }
    }
}
#endif