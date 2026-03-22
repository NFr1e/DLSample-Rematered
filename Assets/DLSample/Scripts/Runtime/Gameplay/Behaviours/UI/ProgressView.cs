using UnityEngine;
using UnityEngine.UI;
using DLSample.Facility.UI;
using DG.Tweening;

namespace DLSample.Gameplay.Behaviours.UI
{
    public class ProgressView : MonoBehaviour
    {
        [SerializeField] private Slider percentageSlider;
        [SerializeField] private LabelDisplayer percentageLabel;
        [SerializeField] private LabelDisplayer gemLabel;

        private GameplayResulter _resulter;
        private Tween _sliderTween;

        private void Awake()
        {
            percentageSlider.minValue = 0;
            percentageSlider.maxValue = 100;
        }
        private void Start()
        {
            _resulter = GameplayEntry.Instance.ServiceLocator.Get<GameplayResulter>();
            Display();
        }
        private void OnDestroy()
        {
            _resulter = null;
            _sliderTween = null;
        }

        private void Display()
        {
            if (_resulter is not null)
            {

                if (percentageSlider)
                {
                    _sliderTween?.Kill();
                    _sliderTween = percentageSlider.DOValue(_resulter.GetPercentage(), 1f).SetEase(Ease.OutExpo);
                }

                if (percentageLabel.label)
                    percentageLabel.SetText($"{_resulter.GetPercentage()}%");

                if (gemLabel.label)
                    gemLabel.SetText($"{_resulter.GetGemsCount()}/{_resulter.LevelData.GemCount}");
            }
        }
    }
}
