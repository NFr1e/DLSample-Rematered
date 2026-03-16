using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DLSample.Facility.UI;

namespace DLSample.Gameplay.Behaviours.UI
{
    public class ProgressView : MonoBehaviour
    {
        [SerializeField] private Slider percentageSlider;
        [SerializeField] private LabelDisplayer percentageLabel;
        [SerializeField] private LabelDisplayer gemLabel;

        private void Start()
        {
            
        }
    }
}
