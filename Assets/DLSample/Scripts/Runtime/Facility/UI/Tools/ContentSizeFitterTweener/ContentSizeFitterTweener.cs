using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace DLSample.Facility.UI
{
    /// <summary>
    ///   ≈‰ƒ⁄»›≥ﬂ¥Á≤¢÷¥––≤πº‰∂Øª≠
    /// </summary>
    public class ContentSizeFitterTweener : MonoBehaviour
    {
        public ContentSizeFitter targetSizeFitter;

        public float tweenDuration = 0.6f;
        public Ease easeType = Ease.OutExpo;

        [HorizontalGroup] public bool horizentalFit = true,verticalFit = true;

        public UnityEvent onFit,onFitted;

        protected RectTransform m_rectTrans;

        private Tween m_sizeFitterTweener;
        private List<ContentSizeFitter> m_childSizeFitters = new();

        public void DoFit()
        {
            StartCoroutine(FitContentSize());
        }
        
        protected void RebuildLayouts()
        {
            foreach (var fitter in m_childSizeFitters)
            {
                if(fitter != null)
                    LayoutRebuilder.ForceRebuildLayoutImmediate(fitter.GetComponent<RectTransform>());
            }
        }

        protected IEnumerator FitContentSize()
        {
            targetSizeFitter.enabled = false;

            RebuildLayouts();

            yield return new WaitForEndOfFrame();

            m_sizeFitterTweener?.Kill();

            onFit?.Invoke();

            Vector2 targetSize = new Vector2
                (horizentalFit ? ContentSize(m_rectTrans).x : m_rectTrans.sizeDelta.x,
                verticalFit ? ContentSize(m_rectTrans).y : m_rectTrans.sizeDelta.y);

            m_sizeFitterTweener = m_rectTrans
                .DOSizeDelta(targetSize,tweenDuration)
                .SetEase(easeType)
                .SetUpdate(true)
                .OnComplete(OnTweenComplete);
        }

        protected virtual void OnTweenComplete()
        {
            targetSizeFitter.enabled = true;

            onFitted?.Invoke();
        }

        public Vector2 ContentSize(RectTransform rectTransform)
        {
            return new(LayoutUtility.GetPreferredWidth(rectTransform), LayoutUtility.GetPreferredHeight(rectTransform));
        }

        protected virtual void Start()
        {
            if(targetSizeFitter == null)
            {
                Debug.LogError($"{gameObject.name}: targetSizeFitter is null");
                return;
            }

            m_childSizeFitters.Clear();

            m_rectTrans = targetSizeFitter.GetComponent<RectTransform>();
            m_childSizeFitters = targetSizeFitter.GetComponentsInChildren<ContentSizeFitter>(true).ToList();
        }
    }
}
