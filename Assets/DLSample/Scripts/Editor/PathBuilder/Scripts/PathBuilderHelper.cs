using UnityEngine;
using DLSample.Editor.PathGrapher;
using DLSample.Gameplay.Behaviours;

namespace DLSample.Editor.PathBuilder
{
    public static class PathBuilderHelper
    {
        #region Path
        public static Transform GeneratePath(PathData pathData, PathGenerateType type, GameObject prefab, float width)
        {
            if (pathData == null || prefab == null) return null;

            Transform pathRoot = new GameObject("PathContainer").transform;

            foreach (var segment in pathData.generatedSegments)
            {
                if (!segment.IsValid) continue;

                foreach (var section in segment.sections)
                {
                    if (section.isTeleport || section.isJump) continue;

                    if (section.points.Length == 2)
                    {
                        CreatePathElement(section.points[0], section.points[1], section.upDir, width, type, prefab, pathRoot);
                    }
                    else
                    {
                        for (int i = 0; i < section.points.Length - 1; i++)
                        {
                            CreatePathElement(section.points[i], section.points[i + 1], section.upDir, width, type, prefab, pathRoot);
                        }
                    }
                }
            }
            return pathRoot;
        }

        private static void CreatePathElement(Vector3 start, Vector3 end, Vector3 up, float width, PathGenerateType type, GameObject prefab, Transform parent)
        {
            Vector3 direction = end - start;
            float distance = direction.magnitude;

            Vector3 position = (start + end) * 0.5f;

            GameObject element = Object.Instantiate(prefab, parent);

            Quaternion rotation = Quaternion.LookRotation(direction, up);
            element.transform.SetLocalPositionAndRotation(position, rotation);

            Vector3 scale = element.transform.localScale;
            scale.x = width;

            switch (type)
            {
                case PathGenerateType.Connected:
                    scale.z = distance + width;
                    break;
                case PathGenerateType.Disconnected:
                    scale.z = distance;
                    break;
            }

            element.transform.localScale = scale;
            element.transform.Translate(-element.transform.up * (0.5f * scale.y + 0.5f), Space.World);

            if (type is PathGenerateType.Disconnected)
            {
                element.transform.Translate(Vector3.back * (width / 2), Space.Self);
            }
        }
        #endregion

        #region Hint
        public static void GenerateHintBox(PathData pathData, GameObject prefab)
        {
            if (pathData == null || prefab == null) return;

            Transform boxRoot = new GameObject("HintBoxes").transform;

            foreach (var segment in pathData.generatedSegments)
            {
                if (!segment.IsValid) continue;

                foreach (var section in segment.sections)
                {
                    if (section.isTeleport || section.isJump) continue;

                    CreateHintBox(section, section.upDir, prefab, boxRoot);
                }
            }
        }

        private static void CreateHintBox(PathSection section, Vector3 upwards, GameObject prefab, Transform root)
        {
            GameObject go = Object.Instantiate(prefab, root);

            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, upwards);
            go.transform.SetLocalPositionAndRotation(section.points[0], rotation);

            if(go.TryGetComponent(out HintBox comp))
            {
                comp.StandardTime = (float)section.startTime;
            }
        }
        #endregion
    }
}
