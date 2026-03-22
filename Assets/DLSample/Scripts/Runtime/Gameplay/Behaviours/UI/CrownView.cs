using DG.Tweening;
using DLSample.Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DLSample.Gameplay.Behaviours.UI
{
    public class CrownView : MonoBehaviour
    {
        [SerializeField] private List<Image> crownIcons = new();
        [SerializeField] private AudioClip crownAudioClip;

        private GameplayResulter _resulter;

        private void Start()
        {
            _resulter = GameplayEntry.Instance.ServiceLocator.Get<GameplayResulter>();
            Display();
        }
        private void Display()
        {
            if (_resulter is null) return;

            for (int i = 0; i < _resulter.GetCrownsCount(); i++)
            {
                if (crownIcons.Count > i)
                {
                    var icon = crownIcons[i];

                    if(icon)
                        icon.DOFade(1, 0.5f);

                    AudioHelper.PlayAudioClip(crownAudioClip);
                }
            }
        }
    }
}
