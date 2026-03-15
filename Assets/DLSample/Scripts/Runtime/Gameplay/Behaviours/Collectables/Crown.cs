using System;
using UnityEngine;
using DG.Tweening;

namespace DLSample.Gameplay.Behaviours
{
    public class Crown : MonoBehaviour, ICollectable
    {
        [SerializeField] private Renderer mRenderer;

        [SerializeField] private GameObject collectEffect;
        [SerializeField] private SpriteRenderer crownIcon;

        public event Action OnCollect;
        public string TypeId => "Collectables.Crown";
        public bool IsCollected { get; private set; } = false;

        private Tweener iconTween;

        private void Awake()
        {
            IsCollected = false;
            crownIcon.DOFade(0, 0);
        }
        public void Collect()
        {
            if (IsCollected) return;

            OnCollected();

            OnCollect?.Invoke();
            IsCollected = true;
        }
        private void OnCollected()
        {
            mRenderer.enabled = false;
            AnimateCollectEffect();
        }
        public void Consume()
        {
            AnimateConsume();
        }

        private void AnimateCollectEffect()
        {
            var effect = Instantiate(collectEffect, mRenderer.transform.position, Quaternion.identity, transform);

            float dist = Vector3.Distance(crownIcon.transform.position, mRenderer.transform.position);
            float duration = 1f;

            Vector3[] path = new[]
            {
                mRenderer.transform.position,
                (crownIcon.transform.position + mRenderer.transform.position) / 2 + Vector3.up * Mathf.Clamp(dist, 2, 10),
                crownIcon.transform.position,
            };

            effect.transform.DOPath(path, duration)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    iconTween?.Kill();
                    iconTween = crownIcon.DOFade(1, 1);

                    Destroy(effect, 0.1f);
                });
        }
        private void AnimateConsume()
        {
            var effect = Instantiate(collectEffect, crownIcon.transform.position, Quaternion.identity, transform);

            iconTween?.Kill();
            iconTween = crownIcon.DOFade(0, 1);

            effect.transform.DOLocalMoveY(effect.transform.position.y + 10, 1).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                Destroy(effect, 0.1f);
            });
        }
    }
}
