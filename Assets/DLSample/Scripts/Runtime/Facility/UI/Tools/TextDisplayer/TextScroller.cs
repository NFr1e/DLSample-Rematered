using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace DLSample.Facility.UI
{
    public class TextScroller : Label
    {
        public Vector2 startPosition;
        [Range(10f, 100f)] public float speed = 10f;
        [Range(0.5f, 3f)] public float delay = 1f;
        public Ease easeType = Ease.Linear;
        public bool autoPlay = true;

        private Text contentText;
        private RectTransform contentTransform;
        private RectTransform containerTransform;
        private Sequence scrollSequence;

        void Awake()
        {
            SetupComponents();
        }

        void SetupComponents()
        {
            containerTransform = GetComponent<RectTransform>();
            contentText = GetComponentInChildren<Text>();

            contentTransform = contentText.GetComponent<RectTransform>();
        }

        public async override void SetText(string text)
        {
            scrollSequence?.Kill();
            contentText.text = text;

            await UniTask.Yield();

            RefreshLayout();
        }

        public void RefreshLayout()
        {
            scrollSequence?.Kill();

            LayoutRebuilder.ForceRebuildLayoutImmediate(contentTransform);

            float textWidth = contentText.preferredWidth;
            float containerWidth = containerTransform.rect.width;

            if (textWidth <= containerWidth)
            {
                SetStaticPosition(textWidth, containerWidth);
            }
            else
            {
                CreateScrollSequence(textWidth, containerWidth);
            }
        }

        void SetStaticPosition(float textWidth, float containerWidth)
        {
            contentTransform.DOAnchorPosX(startPosition.x, 0.3f).SetEase(Ease.OutQuad);
        }

        void CreateScrollSequence(float textWidth, float containerWidth)
        {
            scrollSequence = DOTween.Sequence();

            SetupCenterAlignedScroll(textWidth, containerWidth);
            scrollSequence.SetLoops(-1, LoopType.Restart);
        }

        void SetupCenterAlignedScroll(float textWidth, float containerWidth)
        {
            float offset = textWidth - containerWidth;
            float scrollDuration = offset / speed;

            contentTransform.anchoredPosition = startPosition;

            scrollSequence.Append(contentTransform.DOAnchorPosX(-offset, scrollDuration).SetEase(easeType));
            scrollSequence.AppendInterval(delay);

            scrollSequence.Append(contentTransform.DOAnchorPosX(startPosition.x, scrollDuration).SetEase(easeType));
            scrollSequence.AppendInterval(delay);
        }

        void OnDestroy()
        {
            scrollSequence?.Kill();
        }
    }
}