using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace DLSample.Shared
{
    public class ScreenHelper
    {
        public static Image FullscreenMask(out Canvas canvas)
        {
            Canvas root = new GameObject("MaskCanvas").AddComponent<Canvas>();
            root.renderMode = RenderMode.ScreenSpaceOverlay;
            root.sortingOrder = 100;
            canvas = root;

            CanvasScaler scaler = root.gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(Screen.width, Screen.height);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            Image mask = new GameObject("MaskImg").AddComponent<Image>();
            mask.color = RenderSettings.fogColor;

            RectTransform rect = mask.rectTransform;
            rect.SetParent(root.transform, false);
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = Vector2.zero;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            return mask;
        }
        public static void FullScreenMaskAction(Action action, float inDuration = 0.5f, float outDuration = 0.5f)
        {
            Image mask = FullscreenMask(out Canvas root);
            GameObject.DontDestroyOnLoad(root.gameObject);

            mask.DOFade(1, inDuration).From(0).SetUpdate(true).OnComplete(() =>
            {
                action.Invoke();
                mask.DOFade(0, outDuration).SetUpdate(true).OnComplete(() => GameObject.Destroy(root.gameObject, 0.1f));
            });
        }
        public static async void FullScreenMaskAsyncAction(Func<UniTask> asyncAction, float inDuration = 0.5f, float outDuration = 0.5f)
        {
            Image mask = FullscreenMask(out Canvas root);
            GameObject.DontDestroyOnLoad(root.gameObject);

            await mask.DOFade(1, inDuration).From(0).SetUpdate(true).AsyncWaitForCompletion();
            await asyncAction.Invoke();
            await mask.DOFade(0, outDuration).SetUpdate(true).AsyncWaitForCompletion();

            GameObject.Destroy(root.gameObject);
        }
    }
}
